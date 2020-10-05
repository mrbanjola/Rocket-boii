using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float rocketThrust = 45f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip levelUp;

    // Start is called before the first frame update
    void Start()
    {

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }
    void Update()
    {
        if (state == State.Alive)
        {
            // todo Stop sound when dying.
            RespondToThrustInput();

            RespondToRotateInput();

        }

        resetPosition();

    }

    private void resetPosition()
    {
        if (Input.GetKey(KeyCode.R))
        {
            rigidBody.transform.position = new Vector3(-45, 5, 0);
            rigidBody.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":

                print("okay");
                break;

            case "Finish":

                LevelComplete();
                break;

            default:

                PlayerCrashed();

                break;
        }

    }

    private void LevelComplete()
    {
        state = State.Transcending;
        Invoke("LoadNextLevel", 1f); //parameterise time
        audioSource.PlayOneShot(levelUp);
    }

    private void PlayerCrashed()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        state = State.Dying;
        Invoke("LoadFirstLevel", 2f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);


    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
        

    }

    private void RespondToRotateInput()
    {

        rigidBody.freezeRotation = true;


        float rotationThisFrame;

        if (Input.GetKey(KeyCode.LeftArrow))
        {

            rotationThisFrame = rcsThrust * Time.deltaTime;

            transform.Rotate(Vector3.forward * rotationThisFrame); ;

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * rocketThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }
}