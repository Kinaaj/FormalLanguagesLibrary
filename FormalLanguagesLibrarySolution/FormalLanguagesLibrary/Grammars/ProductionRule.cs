using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace FormalLanguagesLibrary.Grammars
{

    //TODO: ToString()
    //TODO: Class?
    internal readonly record struct ProductionRule<T>
    {
        public Symbol<T>[] LeftHandSide { get;  }
        public Symbol<T>[] RightHandSide { get;  }


        //TODO: How to convert IEnumerable to array?
        public ProductionRule(IEnumerable<T> leftHandSide, IEnumerable<T>[] rightHandSide )
        {
            LeftHandSide = leftHandSide;
            RightHandSide = rightHandSide;
        }

        public bool IsEpsilonRule()
        {
            if (LeftHandSide.Length == 1 && LeftHandSide[0].Type == SymbolType.Epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return (LeftHandSide,RightHandSide).GetHashCode();
        }


        public void _checkInvariants()
        {
            if (LeftHandSide.Length == 0)
            {
                throw new ProductionRuleException("Left-hand side cannot be empty.");
            }
            if(RightHandSide.Length == 0)
            {
                throw new ProductionRuleException("Right-hand side cannot be empty.");
            }
        }

    }

    public class ProductionRuleException : Exception
    {
        public ProductionRuleException(string message) : base(message)
        {

        }
    }
}
