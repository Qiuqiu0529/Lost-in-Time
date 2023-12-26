using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="new Save",menuName ="_Scripts/Save")]
public class Save : ScriptableObject
{
    public int chapterdata;
    public bool onlyStart;
    public bool FaceRight;
    public Vector3 playerLastPos;
 //   public Quaternion playerLastRot;
    public Vector3 playerLastSca;
    public float PlayerLife;
    public Player.PlayerLifeStage lifeStage;
    public float gravityScale;
    public float Rigi2GravityScale;
}
