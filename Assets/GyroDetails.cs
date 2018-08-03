// Create a cube with camera vector names on the faces.
// Allow the device to show named faces as it is oriented.

using UnityEngine;

public class GyroDetails : MonoBehaviour {
	// Faces for 6 sides of the cube
	private GameObject [] quads = new GameObject [6];
	bool calcAttitudeFromGravity = false;

	void Start ()
	{
		Input.backButtonLeavesApp = true;
		Input.gyro.enabled = true;

		// make camera solid colour and based at the origin
		//GetComponent<Camera> ().backgroundColor = new Color (49.0f / 255.0f, 77.0f / 255.0f, 121.0f / 255.0f);
		GetComponent<Camera> ().transform.position = new Vector3 (0, 0, 0);
		GetComponent<Camera> ().clearFlags = CameraClearFlags.SolidColor;

		// create the six quads forming the sides of a cube
		quads [0] = CreateQuad (new Vector3 (1, 0, 0), new Vector3 (0, 90, 0), "plus x", new Color (0.90f, 0.10f, 0.10f, 1));
		quads [1] = CreateQuad (new Vector3 (0, 1, 0), new Vector3 (-90, 0, 0), "plus y", new Color (0.10f, 0.90f, 0.10f, 1));
		quads [2] = CreateQuad (new Vector3 (0, 0, 1), new Vector3 (0, 0, 0), "plus z", new Color (0.10f, 0.10f, 0.90f, 1));
		quads [3] = CreateQuad (new Vector3 (-1, 0, 0), new Vector3 (0, -90, 0), "neg x", new Color (0.90f, 0.50f, 0.50f, 1));
		quads [4] = CreateQuad (new Vector3 (0, -1, 0), new Vector3 (90, 0, 0), "neg y", new Color (0.50f, 0.90f, 0.50f, 1));
		quads [5] = CreateQuad (new Vector3 (0, 0, -1), new Vector3 (0, 180, 0), "neg z", new Color (0.50f, 0.50f, 0.90f, 1));
	}

	// make a quad for one side of the cube
	GameObject CreateQuad (Vector3 pos, Vector3 rot, string aName, Color col)
	{
		GameObject quad = GameObject.CreatePrimitive (PrimitiveType.Quad);
		quad.transform.position = pos;
		quad.transform.rotation = Quaternion.Euler (rot);
		quad.name = aName;
		quad.GetComponent<Renderer> ().material = Instantiate (quad.GetComponent<Renderer> ().material);
		quad.GetComponent<Renderer> ().material.color = col;
		quad.transform.localScale += new Vector3 (0.25f, 0.25f, 0.25f);

		return quad;
	}

	protected void OnGUI ()
	{
		GUI.skin.label.fontSize = Screen.width / 40;
		GUI.skin.toggle.fontSize = Screen.width / 40;
		GUILayout.Label ("  Orientation: " + Screen.orientation);
		GUILayout.Label ("  width/font: " + Screen.width + " : " + GUI.skin.label.fontSize);
		GUILayout.Label ("  attitude: " + Input.gyro.attitude);
		GUILayout.Label ("  gravity: " + Input.gyro.gravity);
		GUILayout.Label ("  userAcceleration: " + Input.gyro.userAcceleration);
		GUILayout.Label ("  rotationRate: " + Input.gyro.rotationRate);
		GUILayout.Label ("  rotationRateUnbiased: " + Input.gyro.rotationRateUnbiased);
		GUILayout.Label ("  updateInterval: " + Input.gyro.updateInterval);
		calcAttitudeFromGravity = GUILayout.Toggle (calcAttitudeFromGravity, "calcAttitudeFromGravity?", "button");
	}

	protected void Update ()
	{
		GyroModifyCamera ();
	}

	/********************************************/

	// The Gyroscope is right-handed.  Unity is left handed.
	// Make the necessary change to the camera.
	void GyroModifyCamera ()
	{
		var q = Input.gyro.attitude;
		if (calcAttitudeFromGravity) {
			var g = Input.gyro.gravity;
			q = Quaternion.FromToRotation (g, new Vector3 (0, 0, -1f));
		}
		transform.rotation = new Quaternion (q.x, q.y, -q.z, -q.w);
	}

}
