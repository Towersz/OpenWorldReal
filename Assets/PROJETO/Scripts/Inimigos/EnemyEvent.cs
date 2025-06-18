 using System;
using UnityEngine;

public static class EnemyEvent
{
    // Evento que qualquer inimigo pode chamar ao morrer
    public static Action<string> OnEnemyKilled;
}
