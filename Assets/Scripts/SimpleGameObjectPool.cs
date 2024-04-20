using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class SimpleGameObjectPool : MonoBehaviour
    {
        [SerializeField] private SimpleGameObjectPoolBase[] _pools;
        [SerializeField] private int _newPoolInitialSize = 5;

        private List<SimpleGameObjectPoolBase> _allPools = new List<SimpleGameObjectPoolBase>();
        private Dictionary<GameObject, SimpleGameObjectPoolBase> _objectPoolsDictionary = new Dictionary<GameObject, SimpleGameObjectPoolBase>();
        public static SimpleGameObjectPool instance;
        private void Awake() {
            instance = this;
        }
        private void Start() {
            Initialize();
        }
        public void Initialize()
        {
            for (int i = 0; i < _pools.Length; i++)
            {
                SimpleGameObjectPoolBase pool = _pools[i];
                pool.ObjectQueue = new Queue<GameObject>();

                for (int j = 0; j < pool.PoolSize; j++)
                {
                    GameObject clone = Instantiate(pool.Prefab, pool.Parent);
                    clone.SetActive(false);

                    _objectPoolsDictionary.Add(clone, pool);
                    pool.ObjectQueue.Enqueue(clone);
                }

                _allPools.Add(pool);
            }
        }

        public GameObject GetObject(GameObject prefab)
        {
            if (prefab == null)
            {
                Debug.LogWarning("Trying to get game object by null prefab!");
                return null;
            }

            SimpleGameObjectPoolBase pool = _allPools.Find(p => p.Prefab == prefab);

            if (pool == null)
            {
                pool = CreateNewPool(prefab);
            }

            if (pool.ObjectQueue.Count > 0)
            {
                GameObject clone = pool.ObjectQueue.Dequeue();
                ResetCloneTransforms(pool.Prefab, clone);

                return clone;
            }
            else
            {
                GameObject clone = Instantiate(pool.Prefab, pool.Parent);
                clone.SetActive(false);

                _objectPoolsDictionary.Add(clone, pool);

                return clone;
            }
        }

        public void ReturnObject(GameObject clone)
        {
            if (_objectPoolsDictionary.TryGetValue(clone, out SimpleGameObjectPoolBase pool))
            {
                pool.ObjectQueue.Enqueue(clone);
                clone.transform.SetParent(pool.Parent);
                clone.SetActive(false);
            }
            else
            {
                // There is no pool for that object.
            }
        }

        public void ReturnAllClones(GameObject prefab)
        {
            SimpleGameObjectPoolBase pool = _allPools.Find(p => p.Prefab == prefab);

            if (pool == null)
            {
                // There is no pool with that prefab.
                return;
            }

            var allObjects = _objectPoolsDictionary.Where(pair => pair.Value == pool).Select(pair => pair.Key).ToList();
            for (int i = 0; i < allObjects.Count; i++)
            {
                if (allObjects[i] != null && allObjects[i].activeSelf)
                {
                    ReturnObject(allObjects[i]);
                }
            }
        }

        private SimpleGameObjectPoolBase CreateNewPool(GameObject prefab)
        {
            SimpleGameObjectPoolBase pool = new SimpleGameObjectPoolBase();
            pool.ObjectQueue = new Queue<GameObject>();
            pool.Prefab = prefab;
            pool.Parent = this.transform;
            pool.PoolSize = _newPoolInitialSize;

            for (int i = 0; i < pool.PoolSize; i++)
            {
                GameObject clone = Instantiate(pool.Prefab, pool.Parent);
                clone.SetActive(false);

                _objectPoolsDictionary.Add(clone, pool);
                pool.ObjectQueue.Enqueue(clone);
            }

            _allPools.Add(pool);
            return pool;
        }

        private void ResetCloneTransforms(GameObject prefab, GameObject clone)
        {
            clone.transform.position = prefab.transform.position;
            clone.transform.rotation = prefab.transform.rotation;
            clone.transform.localScale = prefab.transform.localScale;
        }


        [Serializable]
        private class SimpleGameObjectPoolBase
        {
            [HideInInspector] public Queue<GameObject> ObjectQueue;
            public GameObject Prefab;
            public Transform Parent;
            public int PoolSize;
        }
    }

