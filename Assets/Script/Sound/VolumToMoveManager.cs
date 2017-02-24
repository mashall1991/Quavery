using UnityEngine;
using System.Collections;
using Framework;
public class VolumToMoveManager : Singleton<VolumToMoveManager> {

    public delegate void EvtJump();
    public EvtJump jump;
    
    public delegate void EvtMove();
    public EvtMove move;

    public delegate void EvtStop();
    public EvtStop stop;

    private void Jump(){ if (jump != null) jump();}
    private void Move() { if (move != null) move(); }
    private void Stop() { if (stop != null) stop(); }
    /// <summary>
    /// access a value to control the character
    /// </summary>
    /// <param name="volum"></param>
    public void VolumToMove(float volum)
    {
        if(volum < 0.2f){ Stop();}
        else if(volum >= 0.2f && volum < 0.4f) { Move();}
        else{ Jump();}
    }
}
