//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;

//[CustomEditor (typeof (FieldOfView))]
//public class FieldOfViewEditor : Editor{
    
//    void OnSceneGUI()
//    {
//        FieldOfView fow = (FieldOfView)target;
//        Vector3 fovGround = fow.transform.position;
//        fovGround.y = 0.1f;
//        Handles.color = Color.white;
//        Handles.DrawWireArc(fovGround, Vector3.up, Vector3.forward, 360, fow.viewRadius);
//        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
//        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        
//        Handles.DrawLine(fovGround, fovGround + viewAngleA * fow.viewRadius);
//        Handles.DrawLine(fovGround, fovGround + viewAngleB * fow.viewRadius);

//        Handles.color = Color.red;
//        foreach (Transform visibleTarget in fow.visibleTargets)
//        {
//            Handles.DrawLine(fovGround, visibleTarget.position);
//        }
//    }
//}

