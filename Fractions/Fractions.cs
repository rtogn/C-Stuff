using System;

namespace FractionM
{
    class Fractions
    {
        static void Main(string[] args)
        {
            //Demo code
            Fraction a = new Fraction(1,2);
            Fraction b = new Fraction(-3);
            Fraction c = a - b;
            Console.WriteLine(c);
        }
    }
}

class Fraction
{
    private int numerator { get; set; }
    private int denominator { get; set; }

    public static readonly Fraction ONE = new Fraction(1, 1);
    public static readonly Fraction ZERO = new Fraction(0);

    //**********************************[CONSTRUCTORS]**********************************************
    public Fraction(int numerator)
    {
        // Allow for whole numbers to easily be placed in fractional form without fuss of explicitly defining the denom as 1. 
        this.numerator = numerator;
        this.denominator = 1;
    }
    public Fraction(int numerator, int denominator)
    {
        //_ = numerator / denominator;
        if (denominator == 0) throw new DivideByZeroException();
        if (denominator < 0)
        {
            //Will always transfer sign up given a negative denominator. Given both neum and denom negative the fraction is positive. 
            numerator *= -1;
            denominator *= -1;
        }
        // Divide by GCD for built in reduction of the fraction may take this out in favor of doing it manually.
        int greatestDiv = RandomMath.GCD(numerator, denominator);
        this.numerator = numerator / greatestDiv;
        this.denominator = denominator / greatestDiv;
    }

    //**********************************[BASIC STATIC OPERATRIONS]**********************************************
    public static Fraction operator +(Fraction a)
    {
        // +a = a. +-a = -a.
        return a;
    }
    public static Fraction operator +(Fraction a, Fraction b)
    {
        /*
         * Cross multiply for addition given (a/b) + (c/d).
         * Result = (  (a*d + c*b) / b*d ).
         */
        int resultNumerator = (a.numerator * b.denominator) + (b.numerator * a.denominator);
        int resultDenominator = a.denominator * b.denominator;
        return new Fraction(resultNumerator, resultDenominator);
    }
    public static Fraction operator ++(Fraction a)
    {
        // Simply add 1 by leaving denom and adding it to the num. Eg (1/2)++ = ((1+2)/2).
        return new Fraction((a.numerator + a.denominator), a.denominator);
    }
    public static Fraction operator -(Fraction a)
    {
        // -a means a is negative.
        return new Fraction(-a.numerator, a.denominator);
    }
    public static Fraction operator -(Fraction a, Fraction b)
    {
        /*
         * Cross multiply for subtraction given (a/b) + (c/d).
         * Result = (  (a*d - c*b) / b*d ).
         */
        int resultNumerator = (a.numerator * b.denominator) - (b.numerator * a.denominator);
        int resultDenominator = a.denominator * b.denominator;
        return new Fraction(resultNumerator, resultDenominator);
    }
    public static Fraction operator --(Fraction a)
    {
        return new Fraction((a.numerator - a.denominator), a.denominator);
    }
    public static Fraction operator *(Fraction a, Fraction b)
    {
        int resultNumerator = a.numerator * b.numerator;
        int resultDenominator = a.denominator * b.denominator;
        return new Fraction(resultNumerator, resultDenominator);
    }
    public static Fraction operator /(Fraction a, Fraction b)
    {
        /*
         * Fractional division is multiplication of fraction A with the reciprocal of fraction b. 
         * Eg. (1/2) / (3/2) = (1/2)*(2/3) = 2/6 = 1/3
         */
        return new Fraction((a.numerator * b.denominator), (a.denominator * b.numerator));
    }
    public static Fraction operator %(Fraction a, Fraction b)
    {
        // Mod(a,n) = a - n*floor(a/n) per Donald Knuth. Uses Fraction.Floor method.
        return a - (b * Floor(a / b));
    }
    // Lazy implementations of >,<,<=,>=. 
    public static bool operator >(Fraction a, Fraction b) => a.toDecimal() > b.toDecimal();
    public static bool operator <(Fraction a, Fraction b) => a.toDecimal() < b.toDecimal();
    public static bool operator >=(Fraction a, Fraction b) => a.toDecimal() >= b.toDecimal();
    public static bool operator <=(Fraction a, Fraction b) => a.toDecimal() <= b.toDecimal();
    public static bool operator ==(Fraction a, Fraction b) => (a.numerator == b.numerator) && (a.denominator == b.denominator);
    public static bool operator !=(Fraction a, Fraction b) => !(a == b);

    //**********************************[OTHER OPERATRIONS]**********************************************
    public Fraction reduce(Fraction a)
    {
        //to delete? Built into constructor of new fraction.
        int greatestDiv = RandomMath.GCD(a.numerator, a.denominator);
        return new Fraction((a.numerator / greatestDiv), (a.denominator / greatestDiv));
    }

    public static Fraction Abs(Fraction a)
    {
        // Return fraction representing the Absolute value of the original. 
        return new Fraction(Math.Abs(a.numerator), Math.Abs(a.denominator));
    }

    public static Fraction Pow(Fraction a, int exponent)
    {
        /* Powers of fractions are simple with whole numbers: Take the power of both neum and denom. 
         * Eg. (3/2)^2 = (3^2/2^2) = (9/4).
         * However, if the exponent is negative we will pass to the NegPow function to compute.
         */
        return exponent > 0 ? new Fraction(RandomMath.intPow(a.numerator, exponent), RandomMath.intPow(a.denominator, exponent)) : NegPow(a, exponent);
    }

    public static Fraction Pow(Fraction a, Fraction exponent)
    {
        throw new NotImplementedException(); 
        //   a = Root(a, exponent.denominator);
        //return a;
    }
    public static Fraction NegPow(Fraction a, int exponent)
    {
        /* Given exponent n, variable x. x^-n = 1/(x^n). The same applies to fractions. 
         * Eg. (3/2)^-2 = (1 / (3/2)^2) = 1/(9/4) = (1/1)*(4/9) = (4/9)
         * Uses static variable ONE which is just (1/1)
         */
        Fraction b = ONE / Fraction.Pow(a, Math.Abs(exponent));
        return b;
    }

    public static Fraction Floor(Fraction a)
    {
        /* The floor of a postitive fraction or negative whole number is the integer of it's numerator over the denominator. Eg. Floor(700/6) = 116.  
         * It follows that if the fraction is less than one and positive; the result is Zero. 
         * However, in the case of a negative, non-whole number this does not work. Any negative fration divided out will result in a higher number
         * than the numerator. So we can use this as "detection" by checking the inital floor calculation against the fraction's numerator.
         */
        int floor = a.numerator / a.denominator;
        return (floor > a.numerator) ? new Fraction(floor - 1) : new Fraction(floor);
    }


    public static Fraction Root(Fraction a, int root)
    {
        //not implemented
        throw new NotImplementedException();
        /*
        if a.numerator < 0 {
            throw new NotFiniteNumberException("Illegal use of negative roots. Imaginary numbers not supported.");
        }
        double num = Math.Pow(a.numerator, 1/root);
        double denm = Math.Pow(a.denominator, 1 / root);
        */
    }

    //**********************************[PRINTING & FORMAT OPERATRIONS]**********************************************
    public double toDecimal() => (double)numerator / denominator;
    public static double ToDecimal(Fraction a) => (double)a.numerator / a.denominator;
    public override bool Equals(object obj) => base.Equals(obj);
    public override int GetHashCode() => base.GetHashCode();
    public override string ToString()
    {
        /* String creation done in (num/denom) form. If it is a whole number print in integer form.
         * May remove WN later, it might make it more confusing. 
         * Keep in mind that I chose to move negatives to the numerator, so this should always work given a correctly constructed fraction.
        */
        if (denominator > 1)
        {
            return "(" + numerator + "/" + denominator + ")";
        }
        else
        {
            return "(" + numerator + ")";
        }
    }
}

/*
class IrrationalFraction 
{
    //Under construction. For irrational numbers like pi, e and roots. like pi/2, sqrt(2)/2 etc.
      //Still deciding how to handle while being able to interact with regular Fractions. 
   
}
*/

class RandomMath
{
    public static int GCD(int a, int b)
    {
        /*
         * Very elegant Euclidian Graetest Common Denominator function without recurstion
         * Stolen from Drew Noakes. Thread: https://stackoverflow.com/questions/18541832/c-sharp-find-the-greatest-common-divisor
         * The Final ""mystery"" return will go with whatever item is not zero using a bitwise or. 
         * Ex. 8 is (1000) 4 is (0100): 
         * b>a so b = 1000 % 0100; b = 0000. 
         * 0100(a) | 0000(b) = 0100 (aka 4, the GCD)
         * ADDED ABSOLUTE VALUE TO DEAL WITH NEGATIVE VALUES
         * GCD(a,0) == a
         */
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (a != 0 && b != 0)
        {
            if (a > b)
                a = a % b;
            else
                b = b % a;
        }
        return a | b;
    }

    public static int intPow(int thebase, int exponent)
    {
        return (int)Math.Pow(thebase, exponent);
    }
}