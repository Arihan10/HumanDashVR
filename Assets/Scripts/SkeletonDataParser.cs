using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading.Tasks;

[System.Serializable]
public class SkeletonData {
    public JsonJoint skeleton;
}

[System.Serializable]
public class JsonJoint {
    public JointPos NOSE;
    public JointPos LEFT_WRIST;
    public JointPos RIGHT_WRIST;
    public JointPos LEFT_ANKLE;
    public JointPos RIGHT_ANKLE;
    public JointPos LEFT_HIP;
    public JointPos RIGHT_HIP;
}

[System.Serializable]
public class JointPos {
    public float x;
    public float y;
    public float z;
    public float visibility;
}

public static class SkeletonDataParser {
    private static readonly string API_URL = "http://localhost:8000/get_coordinates";
    private static SkeletonData cachedData = null;

    public static async Task<Dictionary<string, Vector3>> ParseSkeletonData() {
        var jointData = new Dictionary<string, Vector3>(); 

        // Get the latest skeleton data
        SkeletonData data = await FetchSkeletonData(); 
        if (data == null) return jointData; 

        Debug.Log(data); 

        jointData["NOSE"] = new Vector3(data.skeleton.NOSE.x, data.skeleton.NOSE.y, data.skeleton.NOSE.z);
        jointData["LEFT_WRIST"] = new Vector3(data.skeleton.LEFT_WRIST.x, data.skeleton.LEFT_WRIST.y, data.skeleton.LEFT_WRIST.z);
        jointData["RIGHT_WRIST"] = new Vector3(data.skeleton.RIGHT_WRIST.x, data.skeleton.RIGHT_WRIST.y, data.skeleton.RIGHT_WRIST.z);
        jointData["LEFT_ANKLE"] = new Vector3(data.skeleton.LEFT_ANKLE.x, data.skeleton.LEFT_ANKLE.y, data.skeleton.LEFT_ANKLE.z);
        jointData["RIGHT_ANKLE"] = new Vector3(data.skeleton.RIGHT_ANKLE.x, data.skeleton.RIGHT_ANKLE.y, data.skeleton.RIGHT_ANKLE.z);
        jointData["LEFT_HIP"] = new Vector3(data.skeleton.LEFT_HIP.x, data.skeleton.LEFT_HIP.y, data.skeleton.LEFT_HIP.z);
        jointData["RIGHT_HIP"] = new Vector3(data.skeleton.RIGHT_HIP.x, data.skeleton.RIGHT_HIP.y, data.skeleton.RIGHT_HIP.z);

        return jointData;
    }

    public static async Task<float> GetJointVisibility(string jointName) {
        // Get the latest skeleton data
        SkeletonData data = await FetchSkeletonData();
        if (data == null) return 0f;

        switch (jointName) {
            case "NOSE": return data.skeleton.NOSE.visibility;
            case "LEFT_WRIST": return data.skeleton.LEFT_WRIST.visibility;
            case "RIGHT_WRIST": return data.skeleton.RIGHT_WRIST.visibility;
            case "LEFT_ANKLE": return data.skeleton.LEFT_ANKLE.visibility;
            case "RIGHT_ANKLE": return data.skeleton.RIGHT_ANKLE.visibility;
            case "LEFT_HIP": return data.skeleton.LEFT_HIP.visibility;
            case "RIGHT_HIP": return data.skeleton.RIGHT_HIP.visibility;
            default: return 0f;
        }
    }

    private static async Task<SkeletonData> FetchSkeletonData() {
        using (UnityWebRequest request = UnityWebRequest.Get(API_URL)) {
            try {
                // Send the request and wait for response
                var operation = request.SendWebRequest();
                while (!operation.isDone)
                    await Task.Yield();

                if (request.result != UnityWebRequest.Result.Success) {
                    Debug.LogError($"Failed to fetch skeleton data: {request.error}");
                    return null;
                }

                string jsonResponse = request.downloadHandler.text;
                cachedData = JsonUtility.FromJson<SkeletonData>(jsonResponse);
                return cachedData;
            }
            catch (System.Exception e) {
                Debug.LogError($"Error fetching skeleton data: {e.Message}");
                return null;
            }
        }
    }
}