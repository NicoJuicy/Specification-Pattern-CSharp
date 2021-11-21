namespace BuildingBlock.Specification.Helpers
{
    using BuildingBlock.Specification.Extensions;

    public class EnumerableSpecificationHelper<T, TVisitor> where TVisitor : ISpecificationVisitor<TVisitor, T>
    {
        private ISpecification<T, TVisitor> m_specification;

        private ISpecification<T, TVisitor> m_specifications;

        public void Apply()
        {
            if (m_specification == null) return;

            if (m_specifications == null)
            {
                m_specifications = m_specification;
            }
            else
            {
                m_specifications = m_specifications.And(m_specification);
            }

            m_specification = null;
        }

        public ISpecification<T, TVisitor> GetSpecification()
        {
            if (m_specification != null)
            {
                this.Apply();
            }

            return m_specifications;
        }

        public void Include(ISpecification<T, TVisitor> newSpec)
        {
            if (m_specification == null)
                m_specification = newSpec;
            else
                m_specification = m_specification.Or(newSpec);
        }
    }
}
