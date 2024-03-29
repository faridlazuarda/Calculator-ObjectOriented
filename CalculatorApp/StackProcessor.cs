﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/********************************/
/*          EXPRESSION          */
/********************************/
abstract public class Expression
{
    /** DESKRIPSI **/
    /* Expression sebagai base class untuk Terminal, Binary, dan Unary Expression 
    Kelas-kelas turunan dari kelas ini yang akan melakukan operasi pada suatu ekspresi
    matematika, yang secara lebih lanjut akan diproses dalam StackProcessor */

    /** DEFAULT CONSTRUCTOR **/ 
    public Expression() { }

    /** abstract method solve() **/
    abstract public double solve();
}

public class TerminalExpression : Expression
{
    /** KAMUS DATA **/
    protected double x;

    /** DEFAULT CONSTRUCTOR **/
    public TerminalExpression(double x)
    {
        this.x = x;
    }

    /** Implementasi abstract method solve() **/
    public override double solve()
    {
        return this.x;
    }
}

abstract public class UnaryExpression : Expression
{
    /** DESKRIPSI **/
    /* UnaryExpression sebagai base class untuk Positive, Negative, dan Root Expression */
    /* hanya memiliki 1 atribut Expression x */

    /** KAMUS DATA **/
    protected Expression x;

    /** DEFAULT CONSTRUCTOR **/
    public UnaryExpression(Expression x)
    {
        this.x = x;
    }
    abstract public override double solve();
}

public class PositiveExpression : UnaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public PositiveExpression(Expression T) : base(T) { }

    /** Implementasi abstract method solve() **/
    public override double solve()
    {
        return x.solve();
    }
}


public class NegativeExpression : UnaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public NegativeExpression(Expression T) : base(T) { }

    /** Implementasi abstract method solve() **/
    public override double solve()
    {
        return (-1) * x.solve();
    }
}

public class RootExpression : UnaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public RootExpression(Expression T) : base(T) { }

    /** Implementasi abstract method solve() **/
    /* exception handling untuk akar bilangan negatif */
    public override double solve()
    {
        double result = Math.Sqrt(x.solve());
        if (!Double.IsNaN(result))
        {
            return result;
        }
        else
        {
            throw (new NegativeRootException("Exception: Negative value in Root"));
        }

    }
}

abstract public class BinaryExpression : Expression
{
    /** DESKRIPSI **/
    /* Binary sebagai base class untuk Add, Substract, Multiply, dan Divide Expression */
    /* memiliki 2 atribut Expression, x dan y */

    /** KAMUS DATA **/
    protected Expression x;
    protected Expression y;

    /** DEFAULT CONSTRUCTOR **/
    public BinaryExpression(Expression x, Expression y)
    {
        this.x = x;
        this.y = y;
    }
    abstract public override double solve();
}

public class AddExpression : BinaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public AddExpression(Expression x, Expression y) : base(x, y) { }

    /** Implementasi abstract method solve() **/
    public override double solve()
    {
        return x.solve() + y.solve();
    }
}

public class SubstractExpression : BinaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public SubstractExpression(Expression x, Expression y) : base(x, y) { }

    /** Implementasi abstract method solve() **/
    public override double solve()
    {
        return x.solve() - y.solve();
    }
}

public class MultiplyExpression : BinaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public MultiplyExpression(Expression x, Expression y) : base(x, y) { }

    /** Implementasi abstract method solve() **/
    public override double solve()
    {
        return x.solve() * y.solve();
    }
}

public class DivideExpression : BinaryExpression
{
    /** DEFAULT CONSTRUCTOR **/
    public DivideExpression(Expression x, Expression y) : base(x, y) { }

    /** Implementasi abstract method solve() **/
    /* Exception handling untuk pembagian dengan bilangan 0 */
    public override double solve()
    {
        double result;
        result = x.solve() / y.solve();
        if (Double.IsInfinity(result))
        {
            throw (new DivisionByZeroException("Exception: Division by Zero"));
        }
        return result;
    }
}

/********************************/
/*            ELEMEN            */
/********************************/
public class Elemen<T>
{
    /** DESKRIPSI **/
    /* Class Elemen adalah class generic.
    Kelas ini memiliki dua atribut private yakni elmt1 dan elmt2. elmt2 bertipe double,
    Namun untuk elmt1 akan diinstansiasikan menjadi String dan double. Class Elemen ini 
    nantinya akan dipakai menjadi elemen dalam Queue untuk QueueProcessor, serta elemen 
    stack untuk StackProcessor */

    /** KAMUS DATA **/
    protected T elmt1;
    protected double elmt2;

    /** DEFAULT CONSTRUCTOR **/
    public Elemen(T elmt)
    {
        Type param = typeof(T);
        if (typeof(string).IsAssignableFrom(param))
        {
            if (elmt.Equals("+") || elmt.Equals("-") || elmt.Equals("*") || elmt.Equals("/") || elmt.Equals("akar"))
            {
                elmt1 = elmt;
                elmt2 = -1;
            }
            else
            {
                string s = "#";
                elmt1 = (T)(object)s;
                elmt2 = Convert.ToDouble(elmt);
            }
        }
        else
        {
            elmt1 = elmt;
            elmt2 = 1;
        }
    }

    /** GETTER **/
    public T GetItem1()
    {
        return this.elmt1;
    }
    public double GetItem2()
    {
        return this.elmt2;
    }
}

/********************************/
/*     CALCULATOR EXCEPTION     */
/********************************/
public class CalculatorException : Exception
{
    /* CalculatorException merupakan base class untuk ExpressionSyntaxErrorException,
    NegativeRootException, dan DivisionByZeroException */

    public CalculatorException(string message) : base(message)
    {

    }
}
public class ExpressionSyntaxErrorException : CalculatorException
{
    /* ExpressionSyntaxErrorException akan menghandle exception untuk penulisan expression 
    yang salah, misalnya dua operator ditulis secara consecutive */
    public ExpressionSyntaxErrorException(string message) : base(message)
    {

    }
}

public class NegativeRootException : CalculatorException
{
    /* NegativeRootException menghandle exception untuk ekspresi dengan akar dari bilangan
    negatif */
    public NegativeRootException(string message) : base(message)
    {
    }
}

public class DivisionByZeroException : CalculatorException
{
    /* DivisionByZeroException menghandle exception untuk ekspresi yang menghasilkan
    pembagian dengan bilangan 0 */
    public DivisionByZeroException(string message) : base(message)
    {

    }
}

/********************************/
/*         QUEUEPROCESSOR       */
/********************************/
public class QueueProcessor
{
    /* KAMUS DATA */
    Queue<Elemen<string>> expressionInfixQueue;
    Queue<Elemen<string>> expressionPostfixQueue;
    TerminalExpression terminal1, terminal2;
    String expresionOp;
    int terminalState;

    /***** DEFAULT CONSTRUCTOR ******/
    public QueueProcessor()
    {
        /** DESKRIPSI **/


        /** KAMUS LOKAL **/

        /** ALGORITMA **/
        this.expressionInfixQueue = new Queue<Elemen<string>>();
        this.expressionPostfixQueue = new Queue<Elemen<string>>();
    }

    /***** Set Queue to Process *****/
    public void setQueue(Queue<Elemen<string>> queue)
    {
        /** DESKRIPSI **/


        /** KAMUS LOKAL **/

        /** ALGORITMA **/
        this.expressionInfixQueue = queue;
    }

    public double solveQueue()
    {
        /** DESKRIPSI **/
        /* Menyelesaikan ekspresi di dalam queue menjadi sebuah nilai */

        /** KAMUS LOKAL **/
        double result = 0;

        /** ALGORITMA **/
        try
        {
            parseInfixExpression();
            printQueue(this.expressionInfixQueue);
            parseInfixToPostfix();
            result = solvePostfixQueue();
        }
        catch (CalculatorException e)
        {
            throw(e);
        }

        return result;
    }

    private void parseInfixExpression()
    {
        /** DESKRIPSI **/
        /* Me-parse ekspresi infix sekaligus mengecek semantik dari ekspresi tersebut */

        /** KAMUS LOKAL **/
        Queue<Elemen<string>> tempQueue;
        Elemen<string> tempElmt;
        String operatorString;
        int state;  // integer yang melambangkan keadaan pembacaan sekarang
                    // 0 -> pembacaan selesai dan berhasil
                    // 1 -> pembacaan terminal
                    // 2 -> pembacaan operator
        int negCount;   // Penanda banyak minus yang berurutan
        bool insert;

        /** ALGORITMA **/
        tempQueue = new Queue<Elemen<string>>();
        negCount = 0;
        state = 1;  // dimulai dengan pembacaan terminal

        // 1. Loop sampai semua elemen InfixQueue habis
        while (this.expressionInfixQueue.Count != 0)
        {
            // 2. Setiap elemen di-dequeue dan ditangani sesuai state sekarang
            tempElmt = this.expressionInfixQueue.Dequeue();
            switch (state)
            {
                // 2.1  State 1 menandakan state untuk membaca terminal
                //      Kasus yang ada di sini adalah:
                //      1. Jika yang ditemukan terminal, maka berarti tidak ada masalah dan langsung dimasukkan
                //      2. Jika yang ditemukan operator, dicek terlebih dahulu operator apakah itu
                //         Jika ditemukan operator unary (seperti akar), maka diperbolehkan
                //         Kasus jika bertemu operator unary negatif yang pada program ini ditangani operator yang sama
                //         dengan binary pengurangan, dilakukan penandaan bahwa sudah ada 1 minus agar pada pembacaan terminal
                //         selanjutnya nilai negatif dapat dimasukkan ke queue (hanya jika ada 1 minus)
                //      3. Jika yang ditemukan operator, tetapi bukan "-" ataupun "akar", maka secara langsung dapat
                //         disimpulkan sintaks error dalam ekspresi dan dikeluarkan exception
                case 1:
                    //  Penanganan terminal
                    if (tempElmt.GetItem1() == "#")
                    {

                        state = 2;
                        // Penanganan terminal yang sebelumnya operator negatif
                        if (negCount == 1)
                        {
                            Console.WriteLine("neg");
                            tempElmt = new Elemen<string>((tempElmt.GetItem2() * -1).ToString());
                        }
                        Console.WriteLine("angka {0}", tempElmt.GetItem2());
                    }
                    else    // Penanganan Operator
                    {
                        if (tempElmt.GetItem1() == "-" && negCount < 1)
                        {
                            negCount++;
                            continue;
                        }
                        else if (tempElmt.GetItem1() == "akar")
                        {

                        }
                        else
                        {
                            throw (new ExpressionSyntaxErrorException("Syntax Error : Two Consecutive Operators"));
                        }
                    }
                    break;
                // 2.2  State 2 menandakan state untuk membaca operator
                //      State 2 cenderung lebih sederhana karena hanya tinggal membaca operator
                //      Tidak dilakukan pembacaan terminal karena sudah ada prekondisi dari GUI
                //      tidak ada dua terminal yang berurutan
                //      Pada kasus ini hanya me-set state kembali menjadi 1 dan mereset negCount
                case 2:
                    if (tempElmt.GetItem1() != "#")
                    {
                        state = 1;
                        negCount = 0;
                    }
                    break;
            }
            //  3. Elemen yang di dequeue disimpan sementara dalam queue temporary
            tempQueue.Enqueue(tempElmt);


        }

        //  4. Dilakukan pengopian kembali queue yang sudah benar dari temporary ke InfixQueue
        while (tempQueue.Count != 0)
        {
            tempElmt = tempQueue.Dequeue();
            Console.WriteLine("< {0} , {1} >", tempElmt.GetItem1(), tempElmt.GetItem2());
            this.expressionInfixQueue.Enqueue(tempElmt);
        }

        state = 0;

    }

    private void parseInfixToPostfix()
    {
        /** DESKRIPSI **/
        /* Mengubah ekspresi Infix pada queue menjadi Postfix */

        /** KAMUS DATA **/
        Stack<Elemen<string>> operatorStack;     // Stack untuk menyimpan urutan operator
        Elemen<string> queueTemp, stackTemp;
        int operatorPrecedence;

        /** ALGORITMA **/
        operatorStack = new Stack<Elemen<string>>();

        clearQueue(ref expressionPostfixQueue);

        while (this.expressionInfixQueue.Count != 0)
        {
            queueTemp = this.expressionInfixQueue.Dequeue();
            if (queueTemp.GetItem1() != "#")
            {
                if (operatorStack.Count == 0)
                {
                    operatorStack.Push(queueTemp);
                }
                else
                {
                    do
                    {
                        stackTemp = operatorStack.Pop();
                        operatorPrecedence = checkOperatorPrecedence(stackTemp.GetItem1(), queueTemp.GetItem1());
                        if (operatorPrecedence != -1)
                        {
                            if (determineOperatorValue(stackTemp.GetItem1()) == 3 && determineOperatorValue(queueTemp.GetItem1()) == 3)
                            {
                                operatorStack.Push(stackTemp);
                                operatorStack.Push(queueTemp);
                                break;
                            }
                            else
                            {
                                this.expressionPostfixQueue.Enqueue(stackTemp);

                            }
                        }
                        else
                        {
                            operatorStack.Push(stackTemp);
                            operatorStack.Push(queueTemp);
                        }
                    }
                    while (operatorStack.Count != 0 && operatorPrecedence != -1);

                    if (operatorStack.Count == 0)
                    {
                        operatorStack.Push(queueTemp);
                    }

                }
            }
            else
            {
                this.expressionPostfixQueue.Enqueue(queueTemp);
            }
        }

        while (operatorStack.Count != 0)
        {
            stackTemp = operatorStack.Pop();
            this.expressionPostfixQueue.Enqueue(stackTemp);
        }

    }

    private double solvePostfixQueue()
    {
        /** DESKRIPSI **/
        /* Menyelesaikan ekspresi Postfix di queue menjadi nilai */

        /** KAMUS DATA **/
        Stack<TerminalExpression> operationStack;     // Stack untuk menyimpan nilai-nilai operasi
        Elemen<string> queueTemp;
        TerminalExpression term, term1, term2;
        Expression exp;

        /** ALGORITMA **/
        printQueue(this.expressionPostfixQueue);
        operationStack = new Stack<TerminalExpression>();
        while (this.expressionPostfixQueue.Count != 0)
        {
            queueTemp = this.expressionPostfixQueue.Dequeue();
            if (queueTemp.GetItem1() == "#")
            {
                term = new TerminalExpression(queueTemp.GetItem2());
                operationStack.Push(term);
                Console.WriteLine(queueTemp.GetItem2());
            }
            else
            {
                switch (queueTemp.GetItem1())
                {
                    case "+":
                        term2 = operationStack.Pop();
                        term1 = operationStack.Pop();
                        exp = new AddExpression(term1, term2);
                        term = new TerminalExpression(exp.solve());
                        operationStack.Push(term);
                        break;
                    case "-":
                        term2 = operationStack.Pop();
                        term1 = operationStack.Pop();
                        exp = new SubstractExpression(term1, term2);
                        term = new TerminalExpression(exp.solve());
                        operationStack.Push(term);
                        break;
                    case "*":
                        term2 = operationStack.Pop();
                        term1 = operationStack.Pop();
                        exp = new MultiplyExpression(term1, term2);
                        term = new TerminalExpression(exp.solve());
                        operationStack.Push(term);
                        break;
                    case "/":
                        term2 = operationStack.Pop();
                        term1 = operationStack.Pop();
                        exp = new DivideExpression(term1, term2);
                        term = new TerminalExpression(exp.solve());
                        operationStack.Push(term);
                        break;
                    case "akar":
                        term1 = operationStack.Pop();
                        exp = new RootExpression(term1);
                        term = new TerminalExpression(exp.solve());
                        operationStack.Push(term);
                        break;
                }
            }
        }

        term = operationStack.Pop();
        return term.solve();
    }

    public void printQueue(Queue<Elemen<string>> queue)
    {
        Queue<Elemen<string>> tempQueue = new Queue<Elemen<string>>();
        Elemen<string> temp;
        while (queue.Count != 0)
        {
            temp = queue.Dequeue();
            Console.WriteLine("< " + temp.GetItem1() + " , " + temp.GetItem2() + " >");
            tempQueue.Enqueue(temp);
        }

        while (tempQueue.Count != 0)
        {
            temp = tempQueue.Dequeue();
            queue.Enqueue(temp);
        }
    }

    private void clearQueue(ref Queue<Elemen<string>> queue)
    {
        /** DESKRIPSI **/
        /* Mengosongkan Queue sehingga dapat digunakan seperti awal */

        /** KAMUS DATA **/
        Elemen<string> temp;

        /** ALGORITMA **/
        while (queue.Count != 0)
        {
            temp = queue.Dequeue();
        }
    }

    private int checkOperatorPrecedence(string x, string y)
    {
        /** DESKRIPSI **/
        /* Mengecek tingkat presedensi antara operator x dan y */
        /* return value: */
        /*  1 : x > y */
        /* -1 : x < y */
        /*  0 : x = y */

        /** KAMUS LOKAL **/
        int xValue, yValue; // Penanda nilai x dan y secara relatif

        /** ALGORITMA **/
        xValue = determineOperatorValue(x);
        yValue = determineOperatorValue(y);

        if (xValue > yValue)
        {
            return 1;
        }
        else if (xValue < yValue)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    private int determineOperatorValue(string x)
    {
        /** DESKRIPSI **/
        /* Memberikan nilai terhadap operator x secara relatif */
        /* return value: */
        /*  3 : √ */
        /*  2 : * / */
        /*  1 : + - */
        /*  0 : else */

        /** KAMUS LOKAL **/
        int value;

        /** ALGORITMA **/
        switch (x)
        {
            case ("akar"):
                value = 3;
                break;
            case ("*"):
            case ("/"):
                value = 2;
                break;
            case ("+"):
            case ("-"):
                value = 1;
                break;
            default:
                value = 0;
                break;
        }
        return value;
    }

}