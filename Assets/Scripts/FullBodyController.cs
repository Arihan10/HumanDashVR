using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using UnityEngine;
using System.Threading.Tasks;

public class FullBodyController : MonoBehaviour {
    // References to all your IK components
    public TwoBoneIKConstraint leftArmIK;
    public TwoBoneIKConstraint rightArmIK;
    public TwoBoneIKConstraint leftLegIK;
    public TwoBoneIKConstraint rightLegIK;
    public ChainIKConstraint spineIK;
    public ChainIKConstraint hipIK;

    // References to your target objects
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform hipTarget;
    public Transform headTarget;

    // Settings
    public float scaleFactor = 2f;
    public float visibilityThreshold = 0.5f;
    public float interpolationSpeed = 10f;

    // State tracking
    private bool isUpdating = false;

    // Data
    private Dictionary<Transform, Vector3> targetPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, bool> targetVisibility = new Dictionary<Transform, bool>();

    float time;

    private void Awake() {
        // Initialize target positions with current positions
        InitializeTargetPositions();
    }

    private void InitializeTargetPositions() {
        targetPositions[leftHandTarget] = leftHandTarget.position;
        targetPositions[rightHandTarget] = rightHandTarget.position;
        targetPositions[leftFootTarget] = leftFootTarget.position;
        targetPositions[rightFootTarget] = rightFootTarget.position;
        targetPositions[hipTarget] = hipTarget.position;
        targetPositions[headTarget] = headTarget.position;

        // Initialize visibility states
        targetVisibility[leftHandTarget] = false;
        targetVisibility[rightHandTarget] = false;
        targetVisibility[leftFootTarget] = false;
        targetVisibility[rightFootTarget] = false;
        targetVisibility[hipTarget] = false;
        targetVisibility[headTarget] = false;
    }

    private async void Start() {
        // Initial data fetch
        await UpdatePoseFromCV();

        time = Time.time;
    }

    async void Update() {
        InterpolatePositions();

        // Only start a new update if we're not already processing one
        if (!isUpdating) {
            _ = UpdatePoseFromCV();

            Debug.Log(Time.time - time);
            time = Time.time;
        }
    }

    private void InterpolatePositions() {
        float deltaTime = Time.deltaTime;

        foreach (var target in targetPositions.Keys) {
            if (targetVisibility[target]) {
                // Only interpolate if we have valid target data
                target.position = Vector3.Lerp(
                    target.position,
                    targetPositions[target],
                    deltaTime * interpolationSpeed
                );
            }
        }
    }

    private async Task UpdatePoseFromCV() {
        isUpdating = true;

        try {
            // Get the parsed skeleton data
            Dictionary<string, Vector3> jointData = await SkeletonDataParser.ParseSkeletonData();

            Debug.Log(jointData.Count);

            // Update hip position
            if (jointData.ContainsKey("LEFT_HIP") && jointData.ContainsKey("RIGHT_HIP")) {
                float leftHipVisibility = await SkeletonDataParser.GetJointVisibility("LEFT_HIP");
                float rightHipVisibility = await SkeletonDataParser.GetJointVisibility("RIGHT_HIP");

                if (leftHipVisibility > visibilityThreshold && rightHipVisibility > visibilityThreshold) {
                    Vector3 leftHip = jointData["LEFT_HIP"];
                    Vector3 rightHip = jointData["RIGHT_HIP"];
                    Vector3 hipCenter = Vector3.Lerp(leftHip, rightHip, 0.5f);
                    targetPositions[hipTarget] = ConvertToWorldSpace(hipCenter);
                    targetVisibility[hipTarget] = true;
                } else {
                    targetVisibility[hipTarget] = false;
                }
            }

            // Update left hand
            if (jointData.ContainsKey("LEFT_WRIST")) {
                float visibility = await SkeletonDataParser.GetJointVisibility("LEFT_WRIST");
                targetVisibility[leftHandTarget] = visibility > visibilityThreshold;
                if (targetVisibility[leftHandTarget]) {
                    targetPositions[leftHandTarget] = ConvertToWorldSpace(jointData["LEFT_WRIST"]);
                }
            }

            // Update right hand
            if (jointData.ContainsKey("RIGHT_WRIST")) {
                float visibility = await SkeletonDataParser.GetJointVisibility("RIGHT_WRIST");
                targetVisibility[rightHandTarget] = visibility > visibilityThreshold;
                if (targetVisibility[rightHandTarget]) {
                    targetPositions[rightHandTarget] = ConvertToWorldSpace(jointData["RIGHT_WRIST"]);
                }
            }

            // Update left foot
            if (jointData.ContainsKey("LEFT_ANKLE")) {
                float visibility = await SkeletonDataParser.GetJointVisibility("LEFT_ANKLE");
                targetVisibility[leftFootTarget] = visibility > visibilityThreshold;
                if (targetVisibility[leftFootTarget]) {
                    targetPositions[leftFootTarget] = ConvertToWorldSpace(jointData["LEFT_ANKLE"]);
                }
            }

            // Update right foot
            if (jointData.ContainsKey("RIGHT_ANKLE")) {
                float visibility = await SkeletonDataParser.GetJointVisibility("RIGHT_ANKLE");
                targetVisibility[rightFootTarget] = visibility > visibilityThreshold;
                if (targetVisibility[rightFootTarget]) {
                    targetPositions[rightFootTarget] = ConvertToWorldSpace(jointData["RIGHT_ANKLE"]);
                }
            }

            // Update head
            if (jointData.ContainsKey("NOSE")) {
                float visibility = await SkeletonDataParser.GetJointVisibility("NOSE");
                targetVisibility[headTarget] = visibility > visibilityThreshold;
                if (targetVisibility[headTarget]) {
                    targetPositions[headTarget] = ConvertToWorldSpace(jointData["NOSE"]);
                }
            }
        }
        catch (System.Exception e) {
            Debug.LogError($"Error updating pose: {e.Message}");
        }
        finally {
            isUpdating = false;
        }
    }

    private Vector3 ConvertToWorldSpace(Vector3 position) {
        return new Vector3(
            (1 - position.x - 0.5f) * scaleFactor,
            (1 - position.y - 0.5f) * scaleFactor,
            // position.z * scaleFactor / 5f
            0f
        );
    }
}