using UnityEngine;

public class BossArms : MonoBehaviour, IBossable
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float activationDuration = 3.0f;
    [SerializeField] private float returnDuration = 3.0f;
    [SerializeField] private int damageValue;
    
    private Vector3 originalPosition;
    private Vector2 targetPosition;
    
    private float activationTimer = 0.0f;
    private float returnTimer = 0.0f;
    private bool isActivated = false;
    private bool returnRequested = false;

    private void Awake()
    {
        originalPosition = transform.position;
        targetPosition = targetTransform.position;
    }

    [ContextMenu("activate")]
    public void ActivateArms()
    {
        isActivated = true;
        activationTimer = 0.0f;
        returnTimer = 0.0f;
        returnRequested = false;
    }

    [ContextMenu("deactivate")]
    public void DeactivateArms()
    {
        returnRequested = true;
    }

    private void Update()
    {
        if (!isActivated) return;
        if (activationTimer < activationDuration)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            activationTimer += Time.deltaTime;
        }
        else if (returnRequested)
        {
            if (returnTimer < returnDuration)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, step);
                returnTimer += Time.deltaTime;
            }
            else
            {
                isActivated = false;
                activationTimer = 0.0f;
                returnTimer = 0.0f;
                returnRequested = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.TryGetComponent(out IDamagable idamagable))
        {
            idamagable.TakeDamage(damageValue);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.gameObject.TryGetComponent(out IDamagable idamagable))
        {
            idamagable.TakeDamage(damageValue);
        }
    }
}