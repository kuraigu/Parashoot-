using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsGameScene : MonoBehaviour
{
    private static AssetsGameScene _instance;

    [SerializeField]
    private UIAsset _ui;

    [SerializeField]
    private EnemyAsset _enemy;

    public static AssetsGameScene instance
    { get { return _instance; } }

    public UIAsset ui
    { get { return _ui; } }


    public EnemyAsset enemy
    { get { return _enemy; } }

    void Awake()
    {
        _instance = this;
    }

    [System.Serializable]
    public class UIAsset
    {
        [SerializeField] private Canvas _canvas;
        public Canvas canvas
        { get { return _canvas; } }
    }

    [System.Serializable]
    public class EnemyAsset
    {
        [SerializeField]
        private List<Enemy> _enemyList = new List<Enemy>();

        [SerializeField]
        private List<Enemy> _bossList = new List<Enemy>();
        public List<Enemy> enemyList
        { get { return _enemyList; } }

        public List<Enemy> bossList
        { get { return _bossList; } }
    }
}
