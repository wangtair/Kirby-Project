using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClassModeB : ChangeClassMode
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown("p")) Debug.Log("B");
    }
}
