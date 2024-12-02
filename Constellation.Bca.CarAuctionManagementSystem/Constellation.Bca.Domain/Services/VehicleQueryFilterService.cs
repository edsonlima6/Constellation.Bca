using Constellation.Bca.Domain.Entites;
using Constellation.Bca.Domain.Enums;
using Constellation.Bca.Domain.Exceptions;
using Constellation.Bca.Domain.Interfaces.Services;
using Constellation.Bca.Domain.ValueObjects;
using System.Linq.Expressions;
using System.Reflection;

namespace Constellation.Bca.Domain.Services
{
    public class VehicleQueryFilterService : IQueryFilterService<Vehicle>
    {
        private List<string> columnsToParseInt = [nameof(Vehicle.RegistrationYear), nameof(Vehicle.NumberOfSeats), nameof(Vehicle.NumberOfDoors), nameof(Vehicle.Id)];
        private List<string> AllVehicleColumns = [
                nameof(Vehicle.RegistrationYear),
                nameof(Vehicle.NumberOfSeats),
                nameof(Vehicle.NumberOfDoors),
                nameof(Vehicle.Model),
                nameof(Vehicle.Manufacturer),
                nameof(Vehicle.Name),
                nameof(Vehicle.UniqueIdentifier),
                nameof(Vehicle.LoadCapacity),
                nameof(Vehicle.VehicleType),
                nameof(Vehicle.Id),
            ];

        public Expression<Func<Vehicle, bool>> GetExpressionFilter(QueryFilter queryFilter)
        {
            ParameterExpression parameterExp = Expression.Parameter(typeof(Vehicle), "x");
            Expression predicate = Expression.Constant(true);
            MemberExpression memberExp = Expression.Property(parameterExp, queryFilter.Field.ToString());
            BinaryExpression binaryExp;
            ConstantExpression constantExp = GetConstantExpression(queryFilter);

            binaryExp = GetBinaryExpression(queryFilter.Operator, constantExp, memberExp, predicate);
            predicate = Expression.AndAlso(predicate, binaryExp);

            if (queryFilter.Logic != Logic.None && (queryFilter.QueryFilters != null && queryFilter.QueryFilters.Count > 0))
            {
                foreach (var item in queryFilter.QueryFilters)
                {
                    var subSetValue = item.Value ?? string.Empty;
                    var subSetMemberExp = Expression.Property(parameterExp, item.Field.ToString());
                    var subSetConstantExp2 = Expression.Constant(subSetValue);
                    BinaryExpression binaryExp2 = GetBinaryExpression(item.Operator, subSetConstantExp2, subSetMemberExp, predicate);

                    predicate = (queryFilter.Logic == Logic.And) ? Expression.AndAlso(predicate, binaryExp2) : Expression.OrElse(predicate, binaryExp2);
                }
            }
            else
            {
                predicate = Expression.AndAlso(predicate, binaryExp);
            }

            var lambdaExp = Expression.Lambda<Func<Vehicle, bool>>(predicate, parameterExp);
            return lambdaExp;
        }

        private BinaryExpression GetBinaryExpression(Operator filterOperator, ConstantExpression constantExp, MemberExpression memberExp, Expression predicate)
        {

            ParameterExpression parameterExp2 = Expression.Parameter(typeof(Vehicle), "x");
            BinaryExpression binaryExp;

            switch (filterOperator)
            {
                case Operator.Contains:
                    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string), typeof(StringComparison) });
                    var body = Expression.Call(memberExp, method, constantExp, Expression.Constant(StringComparison.OrdinalIgnoreCase));
                    binaryExp = Expression.AndAlso(predicate, body);
                    break;
                case Operator.Equal:
                    binaryExp = Expression.Equal(memberExp, constantExp);
                    break;
                case Operator.NotEqual:
                    binaryExp = Expression.NotEqual(memberExp, constantExp);
                    break;
                case Operator.GreaterThan:
                    binaryExp = Expression.GreaterThan(memberExp, constantExp);
                    break;
                case Operator.GreaterThanOrEqual:
                    binaryExp = Expression.GreaterThanOrEqual(memberExp, constantExp);
                    break;
                case Operator.LessThan:
                    binaryExp = Expression.LessThan(memberExp, constantExp);
                    break;
                case Operator.LessThanOrEqual:
                    binaryExp = Expression.LessThanOrEqual(memberExp, constantExp);
                    break;

                default:
                    binaryExp = Expression.Equal(memberExp, constantExp);
                    break;
            }

            return binaryExp;
        }
    
        private ConstantExpression GetConstantExpression(QueryFilter queryFilter)
        {
            ConstantExpression constantExp;
            var value = queryFilter.Value ?? string.Empty;

            if (columnsToParseInt.Any(x => string.Equals(x, queryFilter.Field.ToString(), StringComparison.InvariantCultureIgnoreCase)))
            {
                ValidateOperator(queryFilter.Operator, true);
                 constantExp = Expression.Constant(Convert.ToInt32(value), typeof(int));
            }
            else if (string.Equals(queryFilter.Field.ToString(), nameof(Vehicle.StartingBid), StringComparison.InvariantCultureIgnoreCase))
            {
                ValidateOperator(queryFilter.Operator, true);
                constantExp = Expression.Constant(Convert.ToDecimal(value), typeof(decimal));
            }
            else if (string.Equals(queryFilter.Field.ToString(), nameof(Vehicle.LoadCapacity), StringComparison.InvariantCultureIgnoreCase))
            {
                ValidateOperator(queryFilter.Operator, true);
                constantExp = Expression.Constant(Convert.ToDouble(value), typeof(double));
            }
            else
            {
                constantExp = Expression.Constant(value);
            }

            if (string.Equals(queryFilter.Field.ToString(), nameof(Vehicle.VehicleType), StringComparison.InvariantCultureIgnoreCase))
            {
                ValidateOperator(queryFilter.Operator, true);
                _ = Enum.TryParse(value, out VehicleType myStatus);
                constantExp = Expression.Constant(myStatus);
            }

            if (string.Equals(queryFilter.Field.ToString(), nameof(Vehicle.VehicleType), StringComparison.InvariantCultureIgnoreCase))
            {
                _ = Enum.TryParse(value, out VehicleType myStatus);
                constantExp = Expression.Constant(myStatus);
            }

            return constantExp;
        }

        private void ValidateOperator(Operator op, bool isNotAllowed)
        {
            if (op == Operator.Contains && isNotAllowed)
                throw new OperatorNotAllowedException($"Invalid operator {Operator.Contains.ToString()} for this type of column");
                
        }
    }
}
