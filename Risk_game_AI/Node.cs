using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk_game_AI
{
    public class Node<T>
    {

        // data is ID
        private T data = default(T);
        private T heuristic = default(T);
        private List<Node<T>> children = new List<Node<T>>();

        private Node<T> parent = null;

        public Node(T data, T heuristic)
        {
            this.data = data;
            this.heuristic = heuristic;
        }

        public void addChild(Node<T> child)
        {
           // if (child != child.parent.parent)
            //{
                child.parent = this;
                this.children.Add(child);
            //}
        }

        public List<Node<T>> getChildren()
        {
            return children;
        }

        public T getData()
        {
            return data;
        }

        public void setData(T data)
        {
            this.data = data;
        }

        public T getHeuristic()
        {
            return heuristic;
        }

        public void setHeuristic(T heuristic)
        {
            this.heuristic = heuristic;
        }

        public void setParent(Node<T> parent)
        {
            this.parent = parent;
        }

        public Node<T> getParent()
        {
            return parent;
        }


    }
}
