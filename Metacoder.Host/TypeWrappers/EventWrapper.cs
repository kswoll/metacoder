using System.Collections.Generic;
using Metacoder.Interfaces;
using Microsoft.CodeAnalysis;

namespace Metacoder.Host.TypeWrappers
{
    public struct EventWrapper : IEvent
    {
        private Compilation compilation;
        private IEventSymbol @event;

        public EventWrapper(Compilation compilation, IEventSymbol @event)
        {
            this.compilation = compilation;
            this.@event = @event;
        }

        public string Name
        {
            get { return @event.Name; }
        }

        public IEnumerable<IAttribute> Attributes
        {
            get { return SymbolHelper.GetAttributes(compilation, @event); }
        }

        public IMethod AddMethod
        {
            get { return new MethodWrapper(compilation, @event.AddMethod); }
        }

        public IMethod RemoveMethod
        {
            get { return new MethodWrapper(compilation, @event.RemoveMethod); }
        }
    }
}