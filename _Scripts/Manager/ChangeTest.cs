using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTest : MonoBehaviour
{
    public enum LifeStage
    {
        CHILD,
        TEEN,
        Adult
    }
    public LifeStage lifeStage;
    bool isactive=false;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (lifeStage)
            {
                case LifeStage.CHILD:
                    ChangeLifeStage.instance.ChangeToChild();
                    break;
                case LifeStage.TEEN:
                    ChangeLifeStage.instance.ChangeToTeen();
                    break;
                case LifeStage.Adult:
                    ChangeLifeStage.instance.ChangeToAdult();
                    break;
            }
        }
    }

}
