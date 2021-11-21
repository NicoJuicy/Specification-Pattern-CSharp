using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Specification.Visitors
{
    public abstract class EFExpressionVisitor<TEntity, TVisitor, TItem>
                         where TVisitor : ISpecificationVisitor<TVisitor, TItem>
    {
        public Expression<Func<TEntity, bool>> Expr { get; protected set; } //; = e => true;

        public abstract Expression<Func<TEntity, bool>> ConvertSpecToExpression(ISpecification<TItem, TVisitor> spec);

      

        public void Visit(AndSpecification<TItem, TVisitor> spec)
        {
            var leftExpr = ConvertSpecToExpression(spec.Left);
            var rightExpr = ConvertSpecToExpression(spec.Right);

            var exprBody = Expression.AndAlso(leftExpr.Body, rightExpr.Body);

            var paramExpr = Expression.Parameter(typeof(TEntity));
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);


            Expr = Expression.Lambda<Func<TEntity, bool>>(exprBody, paramExpr);
        }

        public void Visit(OrSpecification<TItem, TVisitor> spec)
        {
            var leftExpr = ConvertSpecToExpression(spec.Left);
            var rightExpr = ConvertSpecToExpression(spec.Right);

            var exprBody =  Expression.Or(leftExpr.Body, rightExpr.Body);

            var paramExpr = Expression.Parameter(typeof(TEntity));
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);

            Expr = Expression.Lambda<Func<TEntity, bool>>(exprBody, paramExpr);
        }

        public void Visit(NotSpecification<TItem, TVisitor> spec)
        {
            var specExpr = ConvertSpecToExpression(spec.Specification);

            var exprBody = Expression.Not(specExpr.Body);

            var paramExpr = Expression.Parameter(typeof(TEntity));
            exprBody =(UnaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);

            Expr = Expression.Lambda<Func<TEntity, bool>>(exprBody, paramExpr);
        
        }
    }

    /*
     * https://dotnetfiddle.net/04YWR8
    public static class LinqSelectMergeExtension
    {
        /// <summary>
        /// Merges the member initialization list of two lambda expressions into one.
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TBaseDest">Resulting type of the base mapping expression. TBaseDest is
        /// typically a super class of TExtendedDest</typeparam>
        /// <typeparam name="TExtendedDest">Resulting type of the extended mapping expression.</typeparam>
        /// <param name="baseExpression">The base mapping expression, containing a member 
        /// initialization expression.</param>
        /// <param name="mergeExpression">The extended mapping expression to be merged into the
        /// base member initialization expression.</param>
        /// <returns>Resulting expression, after the merged select expression has been applied.</returns>
        public static Expression<Func<TSource, TExtendedDest>> Merge<TSource, TBaseDest, TExtendedDest>(
            this Expression<Func<TSource, TBaseDest>> baseExpression,
            Expression<Func<TSource, TExtendedDest>> mergeExpression)
        {
            // Use an expression visitor to perform the merge of the select expressions.
            var visitor = new MergingVisitor<TSource, TBaseDest, TExtendedDest>(baseExpression);

            return (Expression<Func<TSource, TExtendedDest>>)visitor.Visit(mergeExpression);
        }

        /// <summary>
        /// The merging visitor doing the actual merging work.
        /// </summary>
        /// <typeparam name="TSource">Source data type.</typeparam>
        /// <typeparam name="TBaseDest">Resulting type of the base query.</typeparam>
        /// <typeparam name="TExtendedDest">Resulting type of the merged expression.</typeparam>
        private class MergingVisitor<TSource, TBaseDest, TExtendedDest> : ExpressionVisitor
        {
            /// <summary>
            /// Internal helper, that rebinds the lambda of the base init expression. The
            /// reason for this is that the member initialization list of the base expression
            /// is bound to the range variable in the base expression. To be able to merge those
            /// into the extended expression, all those references have to be rebound to the
            /// range variable of the extended expression. That rebinding is done by this helper.
            /// </summary>
            private class LambdaRebindingVisitor : ExpressionVisitor
            {
                private ParameterExpression newParameter;
                private ParameterExpression oldParameter;

                /// <summary>
                /// Ctor.
                /// </summary>
                /// <param name="newParameter">The range vaiable of the extended expression.</param>
                /// <param name="oldParameter">The range variable of the base expression.</param>
                public LambdaRebindingVisitor(ParameterExpression newParameter,
                    ParameterExpression oldParameter)
                {
                    this.newParameter = newParameter;
                    this.oldParameter = oldParameter;
                }

                /// <summary>
                /// Whenever a memberaccess is done that access the old parameter, rewrite
                /// it to access the new parameter instead.
                /// </summary>
                /// <param name="node">Member expression to visit.</param>
                /// <returns>Possibly rewritten member access node.</returns>
                protected override Expression VisitMember(MemberExpression node)
                {
                    if (node.Expression == oldParameter)
                    {
                        return Expression.MakeMemberAccess(newParameter, node.Member);
                    }
                    return base.VisitMember(node);
                }

                protected override Expression VisitConditional(ConditionalExpression node)
                {

                    return base.VisitConditional(node);
                }

                protected override Expression VisitTypeBinary(TypeBinaryExpression node)
                {
                    if (node.Expression == oldParameter)
                    {
                        switch (node.NodeType)
                        {
                            case ExpressionType.TypeIs:
                                return Expression.TypeIs(newParameter, node.TypeOperand);
                        }

                    }
                    return base.VisitTypeBinary(node);
                }
            }

            private MemberInitExpression baseInit;
            private ParameterExpression baseParameter;
            private ParameterExpression newParameter;

            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="baseExpression">The base expression to merge
            /// into the member init list of the extended expression.</param>
            public MergingVisitor(Expression<Func<TSource, TBaseDest>> baseExpression)
            {
                var lambda = (LambdaExpression)baseExpression;
                baseInit = (MemberInitExpression)lambda.Body;

                baseParameter = lambda.Parameters[0];
            }

            /// <summary>
            /// Pick up the extended expressions range variable.
            /// </summary>
            /// <typeparam name="T">Not used</typeparam>
            /// <param name="node">Lambda expression node</param>
            /// <returns>Unmodified expression tree</returns>
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                if (newParameter == null)
                {
                    newParameter = node.Parameters[0];
                }
                return base.VisitLambda<T>(node);
            }

            /// <summary>
            /// Visit the member init expression of the extended expression. Merge the base 
            /// expression into it.
            /// </summary>
            /// <param name="node">Member init expression node.</param>
            /// <returns>Merged member init expression.</returns>
            protected override Expression VisitMemberInit(MemberInitExpression node)
            {
                LambdaRebindingVisitor rebindVisitor =
                    new LambdaRebindingVisitor(newParameter, baseParameter);

                var reboundBaseInit = (MemberInitExpression)rebindVisitor.Visit(baseInit);

                // allow later merged expression to override the bindings (so we don't have duplicates)

                var mergedInitList = node.Bindings.ToList();
                foreach (var binding in reboundBaseInit.Bindings)
                {
                    var existing = mergedInitList.Any(a => a.Member.Name == binding.Member.Name);
                    // as this starts with the later expression and adds the base expression, we just skip adding for
                    // any property existing
                    if (!existing)
                    {
                        mergedInitList.Add(binding);
                    }

                }

                return Expression.MemberInit(Expression.New(typeof(TExtendedDest)),
                    mergedInitList);
            }
        }
        */

    public class ParameterReplacer : ExpressionVisitor
    {
        private readonly ParameterExpression _parameter;
        private readonly Type _type;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.Type == _type)
            {
                return base.VisitParameter(_parameter);
            }
            return node;
        }

        internal ParameterReplacer(ParameterExpression parameter)
        {
            _type = parameter.Type;
            _parameter = parameter;
        }
    }

   /* public class MemberReplacer : ExpressionVisitor
    {
        private readonly MemberExpression _member;


        protected override Expression VisitMember(MemberExpression node)
        {
            return base.VisitMember(_member);
        }

        internal MemberReplacer(MemberExpression member)
        {
            _member = member;
        }
    }*/
}
