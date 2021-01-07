using UnityEngine;

public class Zombie : MonoBehaviour
{
    public enum Modes
    {
        Passive,
        Active
    }

    #region[Purple] Settings
    public Color[] HairColors;
    public Color[] JacketColors;
    public Color[] ShirtColors;
    public Color[] ShoesColors;
    public Color[] BodyColors;
    public int HairCount;
    public int JacketCount;
    public int ShirtCount;
    public int ShoesCount;
    public Modes Mode;
    public float MoveSpeed;
    public float RotationSpeed;
    public float AttackTime;
    public float TirednessDistance;
    public float TargetValidationMargin;
    public float TargetUpdateTime;
    #endregion Settings

    public bool Attacking { get { return m_attacking; } }

    #region[Blue] Private Members
    private Rigidbody m_rigidbody;
    private Animator m_animator;
    private Character m_targetCharacter;
    private Vector3 m_targetPosition;
    private bool m_movingAlongX = true;
    private bool m_attacking = false;
    #endregion Private Members

    // Start is called before the first frame update
    void Start()
    {
        StartReferences();
        StartAppearance();
    }

    void StartReferences()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();

        InvokeRepeating("UpdateTarget", 0.0f, TargetUpdateTime);
    }

    void StartAppearance()
    {
        string hair = $"Hair{Random.Range(0, HairCount + 1)}";
        string jacket = $"Jacket{Random.Range(0, JacketCount + 1)}";
        string shirt = $"Shirt{Random.Range(0, ShirtCount + 1)}";
        string shoes = $"Shoes{Random.Range(0, ShoesCount + 1)}";

        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform ltransform in transforms)
        {
            if (ltransform.gameObject.name == hair)
            {
                int colorIndex = Random.Range(0, HairColors.Length);
                ltransform.GetComponent<Renderer>()?.material.SetColor("_Color", HairColors[colorIndex]);
            }
            else if (ltransform.gameObject.name == jacket)
            {
                int colorIndex = Random.Range(0, JacketColors.Length);
                ltransform.GetComponent<Renderer>()?.material.SetColor("_Color", JacketColors[colorIndex]);
            }
            else if (ltransform.gameObject.name == shirt)
            {
                int colorIndex = Random.Range(0, ShirtColors.Length);
                ltransform.GetComponent<Renderer>()?.material.SetColor("_Color", ShirtColors[colorIndex]);
            }
            else if (ltransform.gameObject.name == shoes)
            {
                int colorIndex = Random.Range(0, ShoesColors.Length);
                ltransform.GetComponent<Renderer>()?.material.SetColor("_Color", ShoesColors[colorIndex]);
            }
            else if (ltransform.gameObject.name == "Body" || ltransform.gameObject.name == "Head")
            {
                int colorIndex = Random.Range(0, BodyColors.Length);
                ltransform.GetComponent<Renderer>()?.material.SetColor("_Color", BodyColors[colorIndex]);
            }
            else if (
                ltransform.gameObject.name.StartsWith("Hair")
                || ltransform.gameObject.name.StartsWith("Jacket")
                || ltransform.gameObject.name.StartsWith("Shirt")
                || ltransform.gameObject.name.StartsWith("Shoes")
            )
            {
                ltransform.gameObject.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        UpdateMove();
    }

    void UpdateMove()
    {
        if (m_targetCharacter == null)
        {
            m_animator.Play("Idle");

            return;
        }

        if (!m_attacking)
        {
            m_rigidbody.MovePosition(transform.position + ((m_targetPosition - transform.position).normalized * MoveSpeed * Time.fixedDeltaTime));

            m_animator.Play("Walk");
        }
        
        m_rigidbody.MoveRotation(Quaternion.Lerp(m_rigidbody.rotation, Quaternion.LookRotation(m_targetPosition - transform.position), RotationSpeed * Time.fixedDeltaTime));

        float x1 = m_targetPosition.x - TargetValidationMargin;
        float z1 = m_targetPosition.z - TargetValidationMargin;
        float x2 = m_targetPosition.x + TargetValidationMargin;
        float z2 = m_targetPosition.z + TargetValidationMargin;

        if (transform.position.x >= x1 && transform.position.x <= x2 && transform.position.z >= z1 && transform.position.z <= z2)
        {
            m_movingAlongX = !m_movingAlongX;

            CalculateTargetPosition();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Level") Attack();
    }

    public void Attack()
    {
        m_attacking = true;

        m_animator.Play("Attack");
        Invoke("StopAttacking", AttackTime);
    }

    public void StopAttacking()
    {
        m_attacking = false;
    }

    private void UpdateTarget()
    {
        m_targetCharacter = GetClosestTarget(out float distance);

        if (Mode == Modes.Passive && distance >= TirednessDistance)
        {
            m_targetCharacter = null;
        }

        if (m_targetCharacter != null)
        {
            CalculateTargetPosition();
        }
    }

    private Character GetClosestTarget(out float distance)
    {
        var characters = GameObject.FindObjectsOfType<Character>();
        Character closestCharacter = null;
        distance = Mathf.Infinity;

        foreach (Character character in characters)
        {
            float currentDistance = (character.transform.position - transform.position).sqrMagnitude;

            if (currentDistance < distance)
            {
                distance = currentDistance;
                closestCharacter = character;
            }
        }

        return closestCharacter;
    }

    private void CalculateTargetPosition()
    {
        float targetX, targetZ;

        if (m_movingAlongX)
        {
            targetX = Mathf.Floor(m_targetCharacter.transform.position.x / 10.0f) * 10.0f + 5.0f;
            targetZ = Mathf.Floor(transform.position.z / 10.0f) * 10.0f + 5.0f;
        }
        else
        {
            targetX = Mathf.Floor(transform.position.x / 10.0f) * 10.0f + 5.0f;
            targetZ = Mathf.Floor(m_targetCharacter.transform.position.z / 10.0f) * 10.0f + 5.0f;
        }

        m_targetPosition = new Vector3(targetX, transform.position.y, targetZ);
    }
}
