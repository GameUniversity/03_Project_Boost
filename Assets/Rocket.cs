using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] public float rcsThrust = 100f;
    [SerializeField] public float mainThrust = 1.0f;

    public enum State { Alive, Dying, Trancending }

    public State currentState = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {
        if ( currentState == State.Alive )
        {
            Thrust();
            Rotate();
        } else {
            audioSource.Stop();
        }
	}

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust );
            if( !audioSource.isPlaying )
            {
                audioSource.Play();
            }
        }
        else
        {
            if( audioSource.isPlaying )
            {
                audioSource.Stop();
            }
        }
    }

    private void Rotate()
    {

        // take manual control of rotation so that any force applied
        // does not dominate the movement, thus allowing user controlled 
        // recovery
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if ( currentState != State.Alive ) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                currentState = State.Trancending;
                Invoke("LoadNextScene", 1.5f);
                break;
            default:
                // kaboom
                currentState = State.Dying;
                Invoke("LoadFirstScene", 2f);
                break;
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
