using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Transform targetLimb;
    [SerializeField] private ConfigurableJoint m_Configura;

    Quaternion targetInitialRotation;

    private void Start()
    {
        m_Configura = this.GetComponent<ConfigurableJoint>();
        targetInitialRotation = this.targetLimb.transform.localRotation;
    }

    private void Update()
    {
        


    }
    private void FixedUpdate()
    {
        m_Configura.targetRotation = copyRotation();
    }
    private Quaternion copyRotation()
    {
        return Quaternion.Inverse(this.targetLimb.localRotation) * this.targetInitialRotation;
    }
}
