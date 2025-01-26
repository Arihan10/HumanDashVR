using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class JointData {
    public float x;
    public float y;
    public float z;
    public float visibility;
}

[Serializable]
public class SkeletonWrapper {
    public Dictionary<string, JointData> skeleton;
}

// Since Unity's JsonUtility doesn't directly support Dictionary serialization,
// we need this intermediate class
[Serializable]
public class JointEntry {
    public string jointName;
    public JointData jointData;
}

[Serializable]
public class SkeletonData {
    public JointEntry[] joints;
}

public static class SkeletonDataParser {
    private static readonly string jsonData = @"{
    ""skeleton"": {
        ""NOSE"": {
            ""x"": 0.5001883,
            ""y"": 0.08667981,
            ""z"": -0.41663963,
            ""visibility"": 0.9999973
        },
        ""LEFT_EYE_INNER"": {
            ""x"": 0.5068062,
            ""y"": 0.068462834,
            ""z"": -0.3873611,
            ""visibility"": 0.99999166
        },
        ""LEFT_EYE"": {
            ""x"": 0.5127878,
            ""y"": 0.06860229,
            ""z"": -0.38739538,
            ""visibility"": 0.99999166
        },
        ""LEFT_EYE_OUTER"": {
            ""x"": 0.5178808,
            ""y"": 0.06881642,
            ""z"": -0.38742578,
            ""visibility"": 0.99998945
        },
        ""RIGHT_EYE_INNER"": {
            ""x"": 0.48960677,
            ""y"": 0.0695278,
            ""z"": -0.3793472,
            ""visibility"": 0.9999919
        },
        ""RIGHT_EYE"": {
            ""x"": 0.48439002,
            ""y"": 0.07037307,
            ""z"": -0.37934482,
            ""visibility"": 0.99999076
        },
        ""RIGHT_EYE_OUTER"": {
            ""x"": 0.4791974,
            ""y"": 0.07178915,
            ""z"": -0.3792772,
            ""visibility"": 0.9999903
        },
        ""LEFT_EAR"": {
            ""x"": 0.5254082,
            ""y"": 0.08122185,
            ""z"": -0.22427793,
            ""visibility"": 0.9999822
        },
        ""RIGHT_EAR"": {
            ""x"": 0.4735752,
            ""y"": 0.08984584,
            ""z"": -0.18766122,
            ""visibility"": 0.99998856
        },
        ""MOUTH_LEFT"": {
            ""x"": 0.51324934,
            ""y"": 0.1111904,
            ""z"": -0.35961992,
            ""visibility"": 0.9999883
        },
        ""MOUTH_RIGHT"": {
            ""x"": 0.49081293,
            ""y"": 0.11318513,
            ""z"": -0.3485227,
            ""visibility"": 0.9999891
        },
        ""LEFT_SHOULDER"": {
            ""x"": 0.5792445,
            ""y"": 0.21303067,
            ""z"": -0.13962273,
            ""visibility"": 0.9997768
        },
        ""RIGHT_SHOULDER"": {
            ""x"": 0.43206263,
            ""y"": 0.21506695,
            ""z"": -0.049680725,
            ""visibility"": 0.99974513
        },
        ""LEFT_ELBOW"": {
            ""x"": 0.66770494,
            ""y"": 0.25755683,
            ""z"": -0.28156117,
            ""visibility"": 0.9985076
        },
        ""RIGHT_ELBOW"": {
            ""x"": 0.3358221,
            ""y"": 0.24581337,
            ""z"": 0.0058173058,
            ""visibility"": 0.99734193
        },
        ""LEFT_WRIST"": {
            ""x"": 0.6993846,
            ""y"": 0.12554485,
            ""z"": -0.54546624,
            ""visibility"": 0.99366313
        },
        ""RIGHT_WRIST"": {
            ""x"": 0.28706938,
            ""y"": 0.11897018,
            ""z"": -0.085395426,
            ""visibility"": 0.9957616
        },
        ""LEFT_PINKY"": {
            ""x"": 0.7187486,
            ""y"": 0.087194175,
            ""z"": -0.59262997,
            ""visibility"": 0.9660335
        },
        ""RIGHT_PINKY"": {
            ""x"": 0.2661815,
            ""y"": 0.083541065,
            ""z"": -0.11313323,
            ""visibility"": 0.97693133
        },
        ""LEFT_INDEX"": {
            ""x"": 0.71168303,
            ""y"": 0.08000512,
            ""z"": -0.5986653,
            ""visibility"": 0.96620756
        },
        ""RIGHT_INDEX"": {
            ""x"": 0.2720215,
            ""y"": 0.07491279,
            ""z"": -0.12644556,
            ""visibility"": 0.97617257
        },
        ""LEFT_THUMB"": {
            ""x"": 0.70305085,
            ""y"": 0.09234348,
            ""z"": -0.55604076,
            ""visibility"": 0.96427345
        },
        ""RIGHT_THUMB"": {
            ""x"": 0.28036752,
            ""y"": 0.085510686,
            ""z"": -0.097684845,
            ""visibility"": 0.97432226
        },
        ""LEFT_HIP"": {
            ""x"": 0.54457873,
            ""y"": 0.5082238,
            ""z"": -0.03198153,
            ""visibility"": 0.99956965
        },
        ""RIGHT_HIP"": {
            ""x"": 0.45961267,
            ""y"": 0.51162887,
            ""z"": 0.031892236,
            ""visibility"": 0.9996796
        },
        ""LEFT_KNEE"": {
            ""x"": 0.5646393,
            ""y"": 0.71229297,
            ""z"": 0.031391397,
            ""visibility"": 0.9952023
        },
        ""RIGHT_KNEE"": {
            ""x"": 0.44430083,
            ""y"": 0.7163941,
            ""z"": 0.11486532,
            ""visibility"": 0.9974597
        },
        ""LEFT_ANKLE"": {
            ""x"": 0.59739393,
            ""y"": 0.8941781,
            ""z"": 0.24199662,
            ""visibility"": 0.99556494
        },
        ""RIGHT_ANKLE"": {
            ""x"": 0.41682068,
            ""y"": 0.8949324,
            ""z"": 0.35355988,
            ""visibility"": 0.99732333
        },
        ""LEFT_HEEL"": {
            ""x"": 0.5935884,
            ""y"": 0.9215952,
            ""z"": 0.25123775,
            ""visibility"": 0.96062195
        },
        ""RIGHT_HEEL"": {
            ""x"": 0.4317,
            ""y"": 0.9172382,
            ""z"": 0.36618823,
            ""visibility"": 0.9648999
        },
        ""LEFT_FOOT_INDEX"": {
            ""x"": 0.62255836,
            ""y"": 0.9564978,
            ""z"": 0.0640295,
            ""visibility"": 0.99239707
        },
        ""RIGHT_FOOT_INDEX"": {
            ""x"": 0.37844247,
            ""y"": 0.96015877,
            ""z"": 0.19305125,
            ""visibility"": 0.9946989
        }
    }
}";

    private static Dictionary<string, Vector3> jointData;
    private static Dictionary<string, float> visibilityData;

    // Parse the data and return the dictionary
    public static Dictionary<string, Vector3> ParseSkeletonData() {
        if (jointData != null) {
            return jointData;
        }

        jointData = new Dictionary<string, Vector3>();
        visibilityData = new Dictionary<string, float>();

        try {
            // Since JsonUtility doesn't support direct dictionary deserialization,
            // we need to do some manual conversion
            var wrapper = JsonUtility.FromJson<SkeletonWrapper>(jsonData);
            if (wrapper == null || wrapper.skeleton == null) {
                // Try parsing manually
                ParseManually();
                return jointData;
            }

            foreach (var joint in wrapper.skeleton) {
                jointData[joint.Key] = new Vector3(
                    joint.Value.x,
                    joint.Value.y,
                    joint.Value.z
                );
                visibilityData[joint.Key] = joint.Value.visibility;
            }
        }
        catch (Exception e) {
            Debug.LogError($"Error parsing skeleton data: {e.Message}");
            // Fallback to manual parsing
            ParseManually();
        }

        return jointData;
    }

    // Manual parsing as fallback since JsonUtility has limitations with dictionaries
    private static void ParseManually() {
        try {
            // Remove the outer brackets and split by joints
            string skelStr = jsonData.Split(new[] { "\"skeleton\":" }, StringSplitOptions.None)[1].Trim();
            skelStr = skelStr.Trim('{', '}', ' ');

            // Split into individual joint entries
            string[] joints = skelStr.Split(new[] { "}," }, StringSplitOptions.None);

            foreach (string joint in joints) {
                // Clean up the joint string
                string cleanJoint = joint.Trim();
                if (cleanJoint.EndsWith("}")) {
                    cleanJoint = cleanJoint.Substring(0, cleanJoint.Length - 1);
                }

                // Extract joint name
                string[] parts = cleanJoint.Split(new[] { "\":{" }, StringSplitOptions.None);
                if (parts.Length != 2) continue;

                string jointName = parts[0].Trim('"', ' ');
                string values = parts[1];

                // Parse values
                var valueDict = new Dictionary<string, float>();
                string[] valuePairs = values.Split(',');
                foreach (string pair in valuePairs) {
                    string[] keyValue = pair.Split(':');
                    if (keyValue.Length != 2) continue;

                    string key = keyValue[0].Trim('"', ' ');
                    float value;
                    if (float.TryParse(keyValue[1].Trim(), out value)) {
                        valueDict[key] = value;
                    }
                }

                if (valueDict.ContainsKey("x") && valueDict.ContainsKey("y") && valueDict.ContainsKey("z")) {
                    jointData[jointName] = new Vector3(valueDict["x"], valueDict["y"], valueDict["z"]);
                }
                if (valueDict.ContainsKey("visibility")) {
                    visibilityData[jointName] = valueDict["visibility"];
                }
            }
        }
        catch (Exception e) {
            Debug.LogError($"Error in manual parsing: {e.Message}");
        }
    }

    // Get a specific joint position
    public static Vector3 GetJointPosition(string jointName) {
        // Initialize data if not already done
        if (jointData == null) {
            ParseSkeletonData();
        }

        if (jointData.ContainsKey(jointName)) {
            return jointData[jointName];
        }

        Debug.LogWarning($"Joint '{jointName}' not found in skeleton data");
        return Vector3.zero;
    }

    // Get the visibility value for a specific joint
    public static float GetJointVisibility(string jointName) {
        // Initialize data if not already done
        if (visibilityData == null) {
            ParseSkeletonData();
        }

        if (visibilityData.ContainsKey(jointName)) {
            return visibilityData[jointName];
        }

        Debug.LogWarning($"Joint '{jointName}' not found in skeleton data");
        return 0f;
    }
}