using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapShader : MonoBehaviour
{

    public Camera miniMap;

    public Shader EffectShader;



    // Start is called before the first frame update
    void Start()
    {

        miniMap.SetReplacementShader(EffectShader, "Opaque");
    }


    



    // Update is called once per frame
    void Update()
    {
        
    }
}
