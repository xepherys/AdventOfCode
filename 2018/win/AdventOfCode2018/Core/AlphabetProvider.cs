using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Xepherys.AlphabetProviders
{
    #region Interface
    public interface IAlphabetProvider
    {
        IEnumerable<char> GetAlphabet();
        IEnumerable<char> GetAlphabet(int count);
        int CharValue(char c);
    }
    #endregion

    public abstract class AlphabetProvider : IAlphabetProvider
    {
        int max;
        AlphabetCountType type = AlphabetCountType.NONE;

        public abstract IEnumerable<char> GetAlphabet();

        public abstract IEnumerable<char> GetAlphabet(int count);

        public abstract int CharValue(char c);

        #region Properties
        public AlphabetCountType AlphaType
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                CalculateMax();
            }
        }
        #endregion

        protected abstract void CalculateMax();
    }

    #region Implementations
    public class EnglishAlphabetProvider : AlphabetProvider
    {
        #region Fields
        int max = 26;
        string chars = String.Empty;
        AlphabetCountType type = AlphabetCountType.SINGLECASE;
        #endregion

        #region Properties
        public new AlphabetCountType AlphaType
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                CalculateMax();
            }
        }
        #endregion

        #region Primary Methods
        public override IEnumerable<char> GetAlphabet()
        {
            return GetAlphabet(max);
        }
        public override IEnumerable<char> GetAlphabet(int count)
        {
            try
            {
                if (count > max)
                    throw new IAlphabetTooManyCharactersRequestedException(this.GetType().Name.ToString(), count, max);
            }

            catch (IAlphabetTooManyCharactersRequestedException)
            {
                throw;
            }

            int i = 0;

            while (i < count)
            {
                if (i < count && type == AlphabetCountType.LOWERUPPER)
                {
                    for (char c = 'A'; c <= 'Z'; c++)
                    {
                        if (i < count)
                        {
                            chars += Char.ToLower(c);
                            yield return Char.ToLower(c);
                            i++;
                        }
                    }
                }

                for (char c = 'A'; c <= 'Z'; c++)
                {
                    if (i < count && type == AlphabetCountType.INTERMIXEDL)
                    {
                        chars += Char.ToLower(c);
                        yield return Char.ToLower(c);
                        i++;
                    }

                    if (i < count && (type == AlphabetCountType.INTERMIXEDU || type == AlphabetCountType.SINGLECASE 
                        || type == AlphabetCountType.UPPERLOWER || type == AlphabetCountType.INTERMIXEDL || type == AlphabetCountType.LOWERUPPER))
                    {
                        chars += c;
                        yield return c;
                        i++;
                    }

                    if (i < count && type == AlphabetCountType.INTERMIXEDU)
                    {
                        chars += Char.ToLower(c);
                        yield return Char.ToLower(c);
                        i++;
                    }
                }

                if (i < count && type == AlphabetCountType.UPPERLOWER)
                {
                    for (char c = 'A'; c <= 'Z'; c++)
                    {
                        if (i < count)
                        {
                            chars += Char.ToLower(c);
                            yield return Char.ToLower(c);
                            i++;
                        }
                    }
                }
            }
        }

        public override int CharValue(char c)
        {
            return Char.ToUpper(c) - 'A' + 1;
        }
        #endregion

        #region Support Methods
        protected override void CalculateMax()
        {
            switch (this.type)
            {
                case AlphabetCountType.INTERMIXEDL:
                case AlphabetCountType.INTERMIXEDU:
                case AlphabetCountType.LOWERUPPER:
                case AlphabetCountType.UPPERLOWER:
                    this.max = 52;
                    break;
                case AlphabetCountType.SINGLECASE:
                case AlphabetCountType.NONE:
                default:
                    this.max = 26;
                    break;
            }
        }
        #endregion
    }

    public class LargerUniqueAlphabetProvider : AlphabetProvider
    {
        #region Fields
        int max = 61;
        string chars = String.Empty;
        AlphabetCountType type = AlphabetCountType.SINGLECASE;
        #endregion

        #region Properties
        public new AlphabetCountType AlphaType
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                CalculateMax();
            }
        }
        #endregion

        #region Primary Methods
        public override IEnumerable<char> GetAlphabet()
        {
            return GetAlphabet(max);
        }

        public override IEnumerable<char> GetAlphabet(int count)
        {
            try
            {
                if (count > max)
                    throw new IAlphabetTooManyCharactersRequestedException(this.GetType().Name.ToString(), count, max);
            }

            catch (IAlphabetTooManyCharactersRequestedException)
            {
                throw;
            }

            int i = 0;

            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (i < count)
                {
                    yield return c;
                    i++;
                }
            }

            for (char c = 'Ɓ'; c <= 'Ƹ'; c++)
            {
                if (i < count)
                {
                    if (Char.IsUpper(c))
                    {
                        yield return c;
                        i++;
                    }
                }
            }
        }

        public override int CharValue(char c)
        {
            //TODO: Fix for odd values
            return -99;
        }
        #endregion

        #region Support Methods
        protected override void CalculateMax()
        {
            switch (this.type)
            {
                case AlphabetCountType.INTERMIXEDL:
                case AlphabetCountType.INTERMIXEDU:
                case AlphabetCountType.LOWERUPPER:
                case AlphabetCountType.UPPERLOWER:
                    this.max = 61;
                    break;
                case AlphabetCountType.SINGLECASE:
                case AlphabetCountType.NONE:
                default:
                    this.max = 122;
                    break;
            }
        }
        #endregion
    }
    #endregion

    #region Enums
    public enum AlphabetCountType
    {
        NONE            =       0,
        SINGLECASE      =       1,
        INTERMIXEDU     =       2,
        INTERMIXEDL     =       3,
        LOWERUPPER      =       4,
        UPPERLOWER      =       5
    }
    #endregion

    #region Exceptions
    [Serializable]
    public class IAlphabetTooManyCharactersRequestedException : Exception
    {
        public string ResourceReferenceProperty { get; set; }

        public IAlphabetTooManyCharactersRequestedException()
        {
        }

        public IAlphabetTooManyCharactersRequestedException(string requestedProvider, int requestedCharacters, int maxCharacters)
            : base((String.Format("Request to {0} for {1} is invalid. Max characters allowed for provider is: {2}", requestedProvider, requestedCharacters, maxCharacters)))
        {
        }

        public IAlphabetTooManyCharactersRequestedException(string requestedProvider, int requestedCharacters, int maxCharacters, Exception inner)
            : base((String.Format("Request to {0} for {1} is invalid. Max characters allowed for provider is: {2}", requestedProvider, requestedCharacters, maxCharacters)), inner)
        {
        }

        protected IAlphabetTooManyCharactersRequestedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceReferenceProperty = info.GetString("ResourceReferenceProperty");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");
            info.AddValue("ResourceReferenceProperty", ResourceReferenceProperty);
            base.GetObjectData(info, context);
        }
    }
    #endregion
}
