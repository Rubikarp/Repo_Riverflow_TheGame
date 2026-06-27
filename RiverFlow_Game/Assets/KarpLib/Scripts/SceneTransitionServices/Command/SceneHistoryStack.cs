using System.Collections.Generic;
using UnityEngine;

namespace WorldGame.SceneManagement
{
    /// <summary>
    /// Pile pure C# des commandes de navigation. Pas de MonoBehaviour — testable en EditMode.
    /// </summary>
    public class SceneHistoryStack
    {
        private readonly Stack<ISceneCommand> _stack = new Stack<ISceneCommand>();
 
        public bool CanGoBack => _stack.Count > 0 && _stack.Peek().CanUndo;
 
        public int Count => _stack.Count;
 
        public void Push(ISceneCommand command)
        {
            _stack.Push(command);
        }
 
        /// <summary>
        /// Dépile et retourne la commande du dessus. Retourne null si la pile est vide.
        /// </summary>
        public ISceneCommand Pop()
        {
            if (_stack.Count == 0)
            {
                Debug.LogWarning("[SceneHistoryStack] Pop appelé sur une pile vide.");
                return null;
            }
 
            return _stack.Pop();
        }
 
        public ISceneCommand Peek()
        {
            if (_stack.Count == 0)
            {
                return null;
            }
 
            return _stack.Peek();
        }
 
        public void Clear()
        {
            _stack.Clear();
        }
    }
}