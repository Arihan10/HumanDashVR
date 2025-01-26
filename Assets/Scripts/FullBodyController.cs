using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;

public class FullBodyController : MonoBehaviour {
    // References to all your IK components
    public TwoBoneIKConstraint leftArmIK;   // Drag LeftArmIK object here
    public TwoBoneIKConstraint rightArmIK;  // Drag RightArmIK object here
    public TwoBoneIKConstraint leftLegIK;   // Drag LeftLegIK object here
    public TwoBoneIKConstraint rightLegIK;  // Drag RightLegIK object here
    public ChainIKConstraint spineIK;  // Drag SpineIK object here

    // References to your target objects
    public Transform leftHandTarget;   // Drag LeftHand_Target here
    public Transform rightHandTarget;  // Drag RightHand_Target here
    public Transform leftFootTarget;   // Drag LeftFoot_Target here
    public Transform rightFootTarget;  // Drag RightFoot_Target here
    public Transform hipTarget;        // Drag Hip_Target here
    public Transform headTarget;        // Drag Head_Target here

    // Settings
    public float scaleFactor = 2f;

    // This is the method your OpenCV script will call
    public void UpdatePoseFromCV(Dictionary<string, Vector3> jointData) {
        // Convert and apply OpenCV data to targets
        if (jointData.TryGetValue("leftHand", out Vector3 leftHandPos))
            leftHandTarget.position = ConvertToWorldSpace(leftHandPos);

        if (jointData.TryGetValue("rightHand", out Vector3 rightHandPos))
            rightHandTarget.position = ConvertToWorldSpace(rightHandPos);

        if (jointData.TryGetValue("leftFoot", out Vector3 leftFootPos))
            leftFootTarget.position = ConvertToWorldSpace(leftFootPos);

        if (jointData.TryGetValue("rightFoot", out Vector3 rightFootPos))
            rightFootTarget.position = ConvertToWorldSpace(rightFootPos);

        if (jointData.TryGetValue("hip", out Vector3 hipPos))
            hipTarget.position = ConvertToWorldSpace(hipPos);

        if (jointData.TryGetValue("head", out Vector3 headPos))
            headTarget.position = ConvertToWorldSpace(headPos);
    }

    /*private Vector3 ConvertToWorldSpace(Vector3 screenPos) {
        float x = (screenPos.x / Screen.width - 0.5f) * 2f;
        float y = -(screenPos.y / Screen.height - 0.5f) * 2f;
        return new Vector3(x * scaleFactor, y * scaleFactor, 0);
    }*/

    private Vector3 ConvertToWorldSpace(Vector3 position) {
        // Assuming OpenCV sends coordinates in a reasonable range
        return new Vector3(
            position.x * scaleFactor,
            position.y * scaleFactor,
            position.z * scaleFactor
        );
    }
}