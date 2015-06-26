namespace System
{
    public struct Money : IEquatable<Money>,
                          IComparable<Money>,
                          IFormattable,
                          IConvertible
    {
        private const Decimal FractionScale = 1E9M;
        private readonly Currency _currency;
        private readonly Int32 _decimalFraction;
        private readonly Int64 _units;

        public Money(Decimal value)
        {
            checkValue(value);

            _units = (Int64)value;
            _decimalFraction = (Int32)Decimal.Round((value - _units) * FractionScale);

            if (_decimalFraction >= FractionScale)
            {
                _units += 1;
                _decimalFraction = _decimalFraction - (Int32)FractionScale;
            }

            _currency = Currency.FromCurrentCulture();
        }

        public Money(Decimal value, Currency currency)
            : this(value)
        {
            _currency = currency;
        }

        private Money(Int64 units, Int32 fraction, Currency currency)
        {
            _units = units;
            _decimalFraction = fraction;
            _currency = currency;
        }

        public Currency Currency
        {
            get { return _currency; }
        }

        #region Implementation of IEquatable<Money>

        public Boolean Equals(Money other)
        {
            checkCurrencies(other);

            return _units == other._units &&
                   _decimalFraction == other._decimalFraction;
        }

        #endregion

        #region Implementation of IComparable<Money>

        public Int32 CompareTo(Money other)
        {
            checkCurrencies(other);

            Int32 unitCompare = _units.CompareTo(other._units);

            return unitCompare == 0
                       ? _decimalFraction.CompareTo(other._decimalFraction)
                       : unitCompare;
        }

        #endregion

        #region Implementation of IFormattable

        public String ToString(String format, IFormatProvider formatProvider)
        {
            return computeValue().ToString(format, formatProvider);
        }

        #endregion

        #region Implementation of IConvertible

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public Boolean ToBoolean(IFormatProvider provider)
        {
            return _units == 0 && _decimalFraction == 0;
        }

        public Char ToChar(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public SByte ToSByte(IFormatProvider provider)
        {
            return (SByte)computeValue();
        }

        public Byte ToByte(IFormatProvider provider)
        {
            return (Byte)computeValue();
        }

        public Int16 ToInt16(IFormatProvider provider)
        {
            return (Int16)computeValue();
        }

        public UInt16 ToUInt16(IFormatProvider provider)
        {
            return (UInt16)computeValue();
        }

        public Int32 ToInt32(IFormatProvider provider)
        {
            return (Int32)computeValue();
        }

        public UInt32 ToUInt32(IFormatProvider provider)
        {
            return (UInt32)computeValue();
        }

        public Int64 ToInt64(IFormatProvider provider)
        {
            return (Int64)computeValue();
        }

        public UInt64 ToUInt64(IFormatProvider provider)
        {
            return (UInt64)computeValue();
        }

        public Single ToSingle(IFormatProvider provider)
        {
            return (Single)computeValue();
        }

        public Double ToDouble(IFormatProvider provider)
        {
            return (Double)computeValue();
        }

        public Decimal ToDecimal(IFormatProvider provider)
        {
            return computeValue();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        public String ToString(IFormatProvider provider)
        {
            return ((Decimal)this).ToString(provider);
        }

        public Object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotSupportedException();
        }

        #endregion

        public static implicit operator Money(Byte value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(SByte value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(Single value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(Double value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(Decimal value)
        {
            return new Money(value);
        }

        public static implicit operator Decimal(Money value)
        {
            return value.computeValue();
        }

        public static implicit operator Money(Int16 value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(Int32 value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(Int64 value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(UInt16 value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(UInt32 value)
        {
            return new Money((Decimal)value);
        }

        public static implicit operator Money(UInt64 value)
        {
            return new Money((Decimal)value);
        }

        public static Money operator -(Money value)
        {
            return new Money(-value._units, -value._decimalFraction, value._currency);
        }

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw differentCurrencies();
            }

            Int32 fractionSum = left._decimalFraction + right._decimalFraction;

            Int64 overflow = 0;
            Int32 fractionSign = Math.Sign(fractionSum);
            Int32 absFractionSum = Math.Abs(fractionSum);

            if (absFractionSum >= FractionScale)
            {
                overflow = fractionSign;
                absFractionSum -= (Int32)FractionScale;
                fractionSum = fractionSign * absFractionSum;
            }

            Int64 newUnits = left._units + right._units + overflow;

            if (fractionSign < 0 && Math.Sign(newUnits) > 0)
            {
                newUnits -= 1;
                fractionSum = (Int32)FractionScale - absFractionSum;
            }

            return new Money(newUnits,
                             fractionSum,
                             left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw differentCurrencies();
            }

            return left + -right;
        }

        public static Money operator *(Money left, Decimal right)
        {
            return ((Decimal)left * right);
        }

        public static Money operator /(Money left, Decimal right)
        {
            return ((Decimal)left / right);
        }

        public static Boolean operator ==(Money left, Money right)
        {
            return left.Equals(right);
        }

        public static Boolean operator !=(Money left, Money right)
        {
            return !left.Equals(right);
        }

        public static Boolean operator >(Money left, Money right)
        {
            return left.CompareTo(right) > 0;
        }

        public static Boolean operator <(Money left, Money right)
        {
            return left.CompareTo(right) < 0;
        }

        public static Boolean operator >=(Money left, Money right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static Boolean operator <=(Money left, Money right)
        {
            return left.CompareTo(right) <= 0;
        }

        public override Int32 GetHashCode()
        {
            return 207501131 ^ _units.GetHashCode() ^ _currency.GetHashCode();
        }

        public override Boolean Equals(Object obj)
        {
            if (!(obj is Money))
            {
                return false;
            }

            Money other = (Money)obj;
            return Equals(other);
        }

        public override String ToString()
        {
            return computeValue().ToString("C");
        }

        public String ToString(String format)
        {
            return computeValue().ToString(format);
        }

        private Decimal computeValue()
        {
            return _units + _decimalFraction / FractionScale;
        }

        private static Exception differentCurrencies()
        {
            return new InvalidOperationException("Money values are in different " +
                                                 "currencies. Convert to the same " +
                                                 "currency before performing " +
                                                 "operations on the values.");
        }

        private static void checkValue(Decimal value)
        {
            if (value < Int64.MinValue || value > Int64.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value",
                                                      value,
                                                      "Money value must be between " +
                                                      Int64.MinValue + " and " +
                                                      Int64.MaxValue);
            }
        }

        private void checkCurrencies(Money other)
        {
            if (other.Currency != Currency)
            {
                throw differentCurrencies();
            }
        }
    }
}