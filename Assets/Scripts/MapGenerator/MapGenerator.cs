using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Current;

    [SerializeField]
    private List<GameObject> _objMapTemplates;

    [SerializeField]
    private List<GameObject> _objMaps = new List<GameObject>();

    [SerializeField]
    private float _offset;

    [SerializeField]
    private int _currentMapIndex, _currentMapIncrement;
    

    private void Awake()
    {
        Current = this;

        StartGenerating();
        //_currentMapIncrement = 0;
        //GameObject mapObject = Instantiate(_objMapTemplates[0], Vector3.down * _offset * _currentMapIncrement, Quaternion.identity);
        //mapObject.SetActive(true);
        //MapObject mo = mapObject.GetComponentInChildren<MapObject>();
        //mo.SetIndex(0);
        //mo.OnEntered += Mo_OnEntered;
        //_objMaps.Add(mapObject);
        //_currentMapIncrement++;

        //for (int i = 0; i < 3; i++)
        //{
        //    //_currentPosition = Vector3.down * _offset * i;
        //    //_currentMapIncrement = i;
        //    GameObject mapObject = Instantiate(_objMapTemplates[i], Vector3.down * _offset * i, Quaternion.identity);
        //    MapObject mo = mapObject.GetComponent<MapObject>();
        //    //mo._index = i;
        //    //mo.OnEntered += Mo_OnEntered;
        //    _objMaps.Add(mapObject);
        //    mapObject.SetActive(true);
        //}
    }

    private void StartGenerating()
    {
        _currentMapIncrement = 0;
        GameObject mapObject = Instantiate(_objMapTemplates[0], Vector3.down * _offset * _currentMapIncrement, Quaternion.identity);
        mapObject.SetActive(true);
        MapObject mo = mapObject.GetComponentInChildren<MapObject>();
        mo.SetIndex(0);
        mo.OnEntered += Mo_OnEntered;
        _objMaps.Add(mapObject);
        _currentMapIncrement++;
    }

    private void Mo_OnEntered(int index)
    {
        if(_objMaps.Count < 3)
        {
            GameObject mapObject = Instantiate(_objMapTemplates[index + 1], Vector3.down * _offset * _currentMapIncrement, Quaternion.identity);
            mapObject.SetActive(true);
            MapObject mo = mapObject.GetComponentInChildren<MapObject>();
            mo.SetIndex(_objMaps.Count);
            mo.OnEntered += Mo_OnEntered;
            //SpriteRenderer spriteRenderer = mapObject.GetComponent<SpriteRenderer>();
            //spriteRenderer.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            _objMaps.Add(mapObject);
            _currentMapIncrement++;
        }
        else
        {
            if (index + 1 == _objMaps.Count)
                _currentMapIndex = 0;
            else
                _currentMapIndex = index + 1;

            _objMaps[_currentMapIndex].transform.position = Vector3.down * _currentMapIncrement * _offset;
            _currentMapIncrement++;
        }

        
    }

    private void Update()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    _currentPosition = Vector3.down * _offset * i;
        //    _currentMapIncrement = i;
        //    _objMaps[i].transform.position = _currentPosition;
        //}
    }

    public void ResetMapGenerator()
    {
        _objMaps.ForEach(c => Destroy(c));

        StartGenerating();
    }
}
