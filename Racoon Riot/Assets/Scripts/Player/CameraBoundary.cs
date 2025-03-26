using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    [SerializeField] private GameObject _camera, _locRef, _rotRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        RaycastHit hit;
        if(Physics.Linecast(transform.position, _locRef.transform.position, out hit)){
            _camera.transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, hit.point.x, 0.9f),
                Mathf.Lerp(transform.position.y, hit.point.y, 0.9f),
                Mathf.Lerp(transform.position.z, hit.point.z, 0.9f)
            );
        }
        else{
            _camera.transform.position = _locRef.transform.position;
        }
        _camera.transform.LookAt(_rotRef.transform.position);
    }
}
