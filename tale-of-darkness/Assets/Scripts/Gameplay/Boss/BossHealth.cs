using System;

public class BossHealth
{
    public event EventHandler OnDamaged;
    public event EventHandler OnDefeated;

    private int currentHealth;
    private int maxHealth;

    public BossHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public int GetHealth()
    { 
        return currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damageAmount)
    { 
        currentHealth -= damageAmount;
        if (currentHealth < 0) 
            currentHealth = 0;

        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

        if (IsDead())
            if (OnDefeated != null) OnDefeated(this, EventArgs.Empty);
    }

    public bool IsDead()
    { 
        return (currentHealth == 0);
    }
}
