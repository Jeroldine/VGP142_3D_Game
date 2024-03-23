using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5;
    [SerializeField] private Pickups itemDrop;

    private EnemyHealthBar healthBar;
    //private NavMeshAgent agent;
    private float _currentHealth;
    public float health => _currentHealth;

    private bool _isDead = false;

    private bool _isDamaged = false;
    public bool isDamaged => _isDamaged;

    private Animator anim;

    private Coroutine DamagableStateCR;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
        healthBar = GetComponentInChildren<EnemyHealthBar>();
        if (!healthBar) Debug.Log("No Health Bar reference");
        anim = GetComponentInChildren<Animator>();
        //if (!itemDrops) Debug.Log("No Drop Data given for enemy");
    }

    public void TakeDamage(float healthChange)
    {
        if (!_isDamaged)
        {
            _currentHealth -= healthChange;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);
            healthBar.UpdateHealthBar(_currentHealth, maxHealth);
            StartDamageStateChange(1);
        }

        if (_currentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            EnemyDeath();
        }
            
    }

    private void EnemyDeath()
    {
        anim.SetTrigger("Death");
        GetComponent<NavMeshAgent>().speed = 0;
        DropItem();
        DeathEffect();
        Destroy(gameObject, 5);
    }

    private void DropItem()
    {
        Debug.Log("Item Drop not implemented");
        Instantiate(itemDrop, transform.position, transform.rotation);
    }

    private void DeathEffect()
    {
        Debug.Log("Death Effect not implemented");
    }

    public void StartDamageStateChange(float duration)
    {
        if (DamagableStateCR == null)
            DamagableStateCR = StartCoroutine(DamageStateChange(duration));
    }

    IEnumerator DamageStateChange(float duration)
    {
        _isDamaged = true;

        yield return new WaitForSeconds(duration);

        _isDamaged = false;
        DamagableStateCR = null;
    }
}
