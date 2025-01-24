using UnityEngine;

public enum EPlayMode : byte
{
    Standby,
    LinearSeek,
    LinearFlee,
    LinearArrive,
    LinearAvoid
}

public class PlaySceneGameMode : MonoBehaviour
{
    private const float BOUNDARY = 7.0f;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject targetPrefab;
    private GameObject player;
    private GameObject target;
    private float moveSpeed = 2.0f;
    private float turnSpeed = 100.0f;
    private EPlayMode playMode;

    private void Start()
    {
        player = Instantiate<GameObject>(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        target = Instantiate<GameObject>(targetPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        player.SetActive(false);
        target.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            playMode = EPlayMode.Standby;
            player.SetActive(false);
            target.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            playMode = EPlayMode.LinearSeek;
            player.transform.position = GetRandomPosition();
            target.transform.position = GetRandomPosition();
            player.SetActive(true);
            target.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Flee");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Arrive");
        }
        else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Avoid");
        }

        switch (playMode)
        {
            case EPlayMode.Standby:
                break;
            case EPlayMode.LinearSeek:
                LinearSeek();
                break;
            case EPlayMode.LinearFlee:
                break;
            case EPlayMode.LinearArrive:
                break;
            case EPlayMode.LinearAvoid:
                break;
            default:
                break;
        }
    }

    private Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-BOUNDARY, BOUNDARY), 0.0f, Random.Range(-BOUNDARY, BOUNDARY));
    }

    private void LinearSeek()
    {
        Vector3 direction = (target.transform.position - player.transform.position).normalized;
        Vector3 distanceToMove = direction * moveSpeed * Time.deltaTime;
        player.transform.position += distanceToMove;
        Turning();
    }

    private void Turning()
    {
        Vector3 direction = (target.transform.position - player.transform.position).normalized;
        float currentAngle = player.transform.eulerAngles.y;
        float desiredAngle = Vector3.SignedAngle(player.transform.forward, direction, Vector3.up); // Return relative angle
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, currentAngle + desiredAngle, turnSpeed * Time.deltaTime); // 2 absolute angles
        player.transform.rotation = Quaternion.Euler(0.0f, newAngle, 0.0f);
    }
}
