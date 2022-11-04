using System.Collections.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Browser
{
    public class SuccinctSupport
    {

        /// <summary>
      /// Sets the current succinct value for the current thread
      /// </summary>
      /// <param name="succinct"></param>
      /// <remarks></remarks>
        public static void BeginSuccinct(bool succinct)
        {
            var thread = System.Threading.Thread.CurrentThread;

            lock (_succinctStacks)
            {
                Stack<bool> stack;

                if (_succinctStacks.ContainsKey(thread))
                {
                    stack = _succinctStacks[thread];
                }
                else
                {
                    stack = new Stack<bool>();
                    _succinctStacks.Add(thread, stack);
                }
                stack.Push(succinct);
            }
        }

        /// <summary>
      /// Returns the current succinct value for the current thread
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool Current
        {
            get
            {
                var thread = System.Threading.Thread.CurrentThread;

                lock (_succinctStacks)
                    return _succinctStacks.ContainsKey(thread) && _succinctStacks[thread].Peek();
            }
        }

        /// <summary>
      /// Removes the current succinct value valid for the current thread
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool EndSuccinct()
        {
            var thread = System.Threading.Thread.CurrentThread;
            bool retVal;

            lock (_succinctStacks)
            {
                if (_succinctStacks.ContainsKey(thread))
                {
                    var stack = _succinctStacks[thread];
                    int count = stack.Count;

                    if (count > 0)
                    {
                        retVal = stack.Pop();
                        count -= 1;
                    }
                    else
                    {
                        retVal = false;
                    }
                    if (count == 0)
                        _succinctStacks.Remove(thread);
                }
                else
                {
                    retVal = false;
                }
            }

            return retVal;
        }

        private static Dictionary<System.Threading.Thread, Stack<bool>> _succinctStacks = new Dictionary<System.Threading.Thread, Stack<bool>>();

    }
}