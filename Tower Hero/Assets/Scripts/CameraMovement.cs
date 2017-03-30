using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    private Vector3 point;//the coord to the point where the camera looks at
    public float cameraDistanceMax,cameraDistanceMin = 50f,scrollSpeed = 0.5f,spinTime = 3.0f, camera_speed;
    static bool loss;
	Transform cursor;
    float left, right;

    void Start()
    {//Set up things on the start method
        loss = false;
        point = GameObject.Find("Tower 2").transform.position;//get target's coords
        cursor = GameObject.Find("cursor").transform;
        left = Screen.width * 0.04f;
        right = Screen.width * 0.96f;
        //point.y += 12;
        //transform.LookAt(point);//makes the camera look to it
    }

    void FixedUpdate()
    {
		float cursorx = Camera.main.WorldToScreenPoint(cursor.position).x;

		if(cursorx <= left && Lost() == false)
		{
			float dis = Mathf.Abs(left-cursorx);
            Rotate(-0.01f * dis * camera_speed);
        }

        else if(cursorx >= right && Lost() == false)
		{
			float dis = Mathf.Abs(right-cursorx);
            Rotate(0.01f * dis * camera_speed);
        }
        //if(Camera.main.WorldToScreenPoint(cursor.position).y < 5 && Lost() == false)
        //{
        //	Vector3 worldpos = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.WorldToScreenPoint(cursor.position).x,5,0));
        //	cursor.position = new Vector3(worldpos.x, cursor.position.y, worldpos.y);
        //}
    }

    void Rotate(float _speed)
    {
        Vector3 camerapos = Camera.main.WorldToScreenPoint(cursor.position);
        if (camerapos.x < 0)
            camerapos.x = 0;
        else if (camerapos.x>Screen.width)
            camerapos.x = Screen.width;
        transform.RotateAround(point, Vector3.up, _speed);
        cursor.position = Camera.main.ScreenToWorldPoint(camerapos);

    }

    void Update()
    {//makes the camera rotate around "point" coords, rotating around its Y axis, 20 degrees per second times the speed modifier
        //transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * camera_speed);
        if ((Input.GetButton("Rotation") || Input.GetButton("Rotation1")) || Input.GetAxis("Rotation1") != 0 && !Lost() && FlashScreen.Flashing() == false && Time.timeScale != 0)
        {
            if (Input.GetButton("Rotation"))
                Rotate(Input.GetAxis("Mouse X") * camera_speed);
            else
                Rotate(Input.GetAxisRaw("Rotation1") * camera_speed);
        }
        ////Mouse
        //else if (Input.GetAxis("Mouse ScrollWheel") != 0 && CameraShake.Is_Shaking() == false &&FlashScreen.Flashing() == false && !Input.GetButton("Rotation") && Time.timeScale != 0)
        //{
        //    cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        //    cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        //    // set camera position
        //    Vector3 position = transform.rotation * new Vector3(0.0f, 0.0f, -cameraDistance) + target.transform.position;

        //    transform.position = Vector3.Lerp(transform.position, position, (scrollSpeed / 10) * Time.deltaTime);
        //}
        ////Keyboard
        //else if (Input.GetButton("Zoom") && CameraShake.Is_Shaking() == false &&FlashScreen.Flashing() == false && Time.timeScale != 0)
        //{
        //    cameraDistance -= Input.GetAxis("Zoom") * scrollSpeed;
        //    cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        //    // set camera position
        //    Vector3 position = transform.rotation * new Vector3(0.0f, 0.0f, -cameraDistance) + target.transform.position;

        //    transform.position = Vector3.Lerp(transform.position, position, (scrollSpeed / 10) * Time.deltaTime);
        //}
        ////Controller
        //else if (Input.GetAxis("Zoom2") != 0 && CameraShake.Is_Shaking() == false &&FlashScreen.Flashing() == false && Time.timeScale != 0)
        //{
        //    cameraDistance -= Input.GetAxis("Zoom2") * scrollSpeed;
        //    cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);

        //    // set camera position
        //    Vector3 position = transform.rotation * new Vector3(0.0f, 0.0f, -cameraDistance) + target.transform.position;

        //    transform.position = Vector3.Lerp(transform.position, position, (scrollSpeed / 10) * Time.deltaTime);
        //}
    }

	IEnumerator LoseSpin()
	{
        //Time.timeScale = 0;
        loss = true;
        GameObject.Find("mana").SetActive(false);
        while (spinTime > 0)
		{
            spinTime -= Time.unscaledDeltaTime;
			//FindObjectOfType<CameraMovement>().Spin();
			Spin();
			yield return null;			
		}

		if(spinTime <= 0.0f)
		{
			LoadLose();
		}
	}

	public void Lose ()
{
        GameObject.Find("GameController").GetComponent<ChangeMusic>().SwitchMusic((int)ChangeMusic.Music.Lose);
		StartCoroutine("LoseSpin");
}
	public void LoadLose()
	{
        Time.timeScale = 1;
		UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScene");
	}
	public void Spin()
	{
		transform.RotateAround(point, Vector3.up, 0.25f * camera_speed);
	}

    static public bool Lost()
    {
        return loss;
    }
}
