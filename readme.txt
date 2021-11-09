Fraction written by R. Togn. rtognoni1@student.gsu.edu 3/9/2021

Description: Creates a way to do base math operations in a purely fractional form. Still would require a 
wrapper to make use easy without explicitly defining a new Fraction instance. 
Originally made for Calculus class as answers typically had to be maintained in fractional form.

Is fairly robust and can handle +,-,%,/,*, exponents and inc/decrement ops (++,--). 
Fracitons are reduced by default. Negative values are either canceled or passed to the numerator. 
However is not set up to handle direct interaction with basic types like integers. The number in
question will have to be converted to a Fraction first. 

Example:
            Fraction a = new Fraction(1,2); // (1/2) Numerator, denominator format 
            Fraction b = new Fraction(-3); //(-3/1) Denominator not required for whole numbers
            Fraction c = a - b;
            Console.WriteLine(c);
Output: (7/2)

classes: Fraction, RandomMath (poorly named, just a container for a few functions like GCD).

Credit for certain algorithms is given in the comments of the given function. 
