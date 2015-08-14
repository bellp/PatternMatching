using System;

namespace Sample.Model
{
    class Fruit
    {
        private readonly string _description;

        public string Description
        {
            get { return _description; }
        }

        public Fruit(string description)
        {
            if (description == null) throw new ArgumentNullException("description");
            _description = description;
        }

        protected bool Equals(Fruit other)
        {
            return string.Equals(_description, other._description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Fruit)obj);
        }

        public override int GetHashCode()
        {
            return (_description != null ? _description.GetHashCode() : 0);
        }
    }
}
