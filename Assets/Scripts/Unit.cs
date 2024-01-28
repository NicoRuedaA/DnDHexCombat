using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using UnityEngine.UI;


[SelectionBase]
public class Unit : MonoBehaviour
{

    [SerializeField] private int _movementPoints = 20, _initiative = 0, _initiativeResult;

    private bool _isAttacking;

    public bool IsAttacking()
    {
        print(_isAttacking);
        return _isAttacking;
    }
    
    private HealthBar _healtBar;
    
    public int MovementPoints
    {
        get => _movementPoints;
    }

    public int Initiative
    {
        get => _initiative;
    }
    
    public int InitiativeResult
    {
        get => _initiativeResult;
    }


    [SerializeField] private float movementDuration = 1, rotationDuration = 0.3f;

    //cuidado mariano
    private GlowHighlight _glowHighlight;
    private Queue<Vector3> _pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    private void Awake()
    {
        _glowHighlight = GetComponent<GlowHighlight>();
        _healtBar = GetComponentInChildren<HealthBar>();



    }

    private void Start()
    {
        MoveToClosestHex();
        _isAttacking = false;
    }

    void MoveToClosestHex()
    {
        Vector3Int newPositionGrid = HexGrid.instance.GetClosestHex(Vector3Int.FloorToInt(transform.position));
        Hex newPositionHex = HexGrid.instance.GetTileAt(newPositionGrid);
        transform.position = newPositionHex.transform.position;
    }
    
    internal void Deselect()
    {
        //Debug.Log("unidad se deselecciona");
        _glowHighlight.ToggleGlow(false);
    }

    public void Select()
    {
        //Debug.Log("unidad se selecciona");
        _glowHighlight.ToggleGlow(true);

    }

    internal void MoveThroughPath(List<Vector3> currentPath)
    {
        _pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = _pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }

    private IEnumerator RotationCoroutine(Vector3 endPosition, float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation, endRotation)), 1.0f) == false)
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / this.rotationDuration; // 0-1
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }

            transform.rotation = endRotation;
        }

        StartCoroutine(MovementCoroutine(endPosition));

    }

    private IEnumerator MovementCoroutine(Vector3 endPosition)
    {
        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }

        transform.position = endPosition;

        if (_pathPositions.Count > 0)
        {
            Debug.Log("selecting next position");
            StartCoroutine(RotationCoroutine(_pathPositions.Dequeue(), rotationDuration));
        }


        else
        {
            Debug.Log("mov finished");
            MovementFinished?.Invoke(this);
            CombatManager.instance.NextTurn();
        }
    }

    public int RollDices(playerStats toRoll)
    {
        Random rnd = new Random();
        int dice  = rnd.Next(1, 21);  
        int result;
        switch (toRoll)
        {
            case playerStats.initative:
            {
                result = _initiative + dice;
                print( gameObject.transform.name + $" gets {dice} on dices");
                print( gameObject.transform.name + $" gets {result} of initiative");
                _initiativeResult = result;
                
                break;
            }
            default:
                throw new Exception($"Player stat of type {toRoll} not supported");
        }

        return result;
    }

    public bool IsMyTurn() => gameObject == CombatManager.instance.TurnList[CombatManager.instance.TurnNumber()].gameObject;
    



    internal int Damage, Health, MAX_HEALTH;
    

    public void Attack(Unit unitReference)
    {
        unitReference.ReceiveAttack(this.Damage);
    }

    public void ReceiveAttack(int damage)
    {
        Health -= damage;
        _healtBar.UpdateHealth();
        if(Health<= 0) Destroy(this);
        
    }


}

