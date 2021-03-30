C# style guide
==============

This page is based on the Godot coding style guide by Juan Linietsky, Ariel Manzur and the Godot community which is
`CC-BY-3.0 licensed <https://github.com/godotengine/godot-docs/blob/master/LICENSE.txt>`_.
The `document reference <https://github.com/godotengine/godot-docs/blob/master/getting_started/scripting/c_sharp/c_sharp_style_guide.rst>`_
is followed by developers of and contributors to Godot itself and summarizes most of the Microsoft
`C# Coding Conventions <https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions>`_ and
`Framework Design Guidelines <https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines>`_.

We've added the organisation part of the
`Googles C# Style Guide <https://google.github.io/styleguide/csharp-style.html>`_ which is
`CC-BY-3.0 licensed <https://creativecommons.org/licenses/by/3.0/>`_
and simplified and less strict parts for our project needs.


Layout
------

Using declarations go at the top, before any namespaces.

**Use the following order:**

* Nested classes, enums, delegates and events.
* Constant, Static and readonly fields.
* Fields.
* Properties.
* Constructors and finalizers.
* Methods.
* (Standard serialisation)

**Within each group, elements should be in the following order:**

* Public.
* Internal, Protected and Private.

Where possible, group interface implementations together.


Formatting
----------

General guidelines
~~~~~~~~~~~~~~~~~~

* Use line feed (**LF**) characters to break lines, not CRLF or CR.
* Use one line feed character at the end of each file, except for `csproj` files.
* Use **UTF-8** encoding without a `byte order mark <https://en.wikipedia.org/wiki/Byte_order_mark>`_.
* Use **4 spaces** instead of tabs for indentation (which is referred to as "soft tabs").


Line breaks and blank lines
~~~~~~~~~~~~~~~~~~~~~~~~~~~

For a general indentation rule, follow `the "Allman Style" <https://en.wikipedia.org/wiki/Indentation_style#Allman_style>`_
which recommends placing the brace associated with a control statement on the next line, indented to
the same level:

.. code-block:: csharp

    // Use this style:
    if (x > 0)
    {
        DoSomething();
    }

    // NOT this:
    if (x > 0) {
        DoSomething();
    }

However, you may choose to omit line breaks inside brackets:

* For simple property accessors.
* For simple object, array, or collection initializers.
* For abstract auto property, indexer, or event declarations.

Or if it heavily simplifies the code part with an obvious structure.

.. code-block:: csharp

    // You may put the brackets in a single line in following cases:
    public interface MyInterface
    {
        int MyProperty { get; set; }
    }

    public class MyClass : ParentClass
    {
        public int Value
        {
            get { return 0; }
            set
            {
                ArrayValue = new [] {value};
            }
        }
    }


Insert two blank lines between sections. Sections can be marked with regions as a visual aid.

* After a list of ``using`` statements and the next section.
* After a list of properties and the next section.
* After a list of fields and the next section.
* After constructors and finalizers and the next section.
* After public methods and the next section.

Insert a blank line:

* Between method, properties, and inner type declarations.
* At the end of each file.

Field and constant declarations can be grouped together according to relevance. In that case, consider
inserting a blank line between the groups for easier reading.

Avoid inserting a blank line:

* After ``{``, the opening brace.
* Before ``}``, the closing brace.
* After a comment block or a single-line comment.
* Adjacent to another blank line.

.. code-block:: csharp

    using System;
    using Godot;
    
                                              // 2 blank lines after `using` list.
    public class MyClass
    {                                         // No blank line after `{`.
        public enum MyEnum
        {
            Value,
            AnotherValue                      // No blank line before `}`.
        }
                                              // Blank line around inner types.
        public const int SomeConstant = 1;
        public const int AnotherConstant = 2;
        
        private Vector3 _x;                  // Related constants or fields can be
        private Vector3 _y;                  // grouped together.
        
        private float _width;
        private float _height;
        
        public int MyProperty { get; set; }
                                              // 2 blank lines between sections.
        
        public void MyMethod()
        {
            // Some comment.
            AnotherMethod();                  // No blank line after a comment.
        }
                                              // Blank line around methods.
        public void AnotherMethod()
        {
        }
        
        
        private void ThirdMethod()
        {
        }
    }


Using spaces
~~~~~~~~~~~~

Insert a space:

* Around a binary and tertiary operator.
* Between an opening parenthesis and ``if``, ``for``, ``foreach``, ``catch``, ``while``, ``lock`` or ``using`` keywords.
* Before and within a single line accessor block.
* Between accessors in a single line accessor block.
* After a comma which is not at the end of a line.
* After a semicolon in a ``for`` statement.
* After a colon in a single line ``case`` statement.
* Around a colon in a type declaration.
* Around a lambda arrow.
* After a single-line comment symbol (``//``), and before it if used at the end of a line.

Do not use a space:

* After type cast parentheses.
* Within single line initializer braces.

The following example shows a proper use of spaces, according to some of the above mentioned conventions:

.. code-block:: csharp

    public class MyClass<A, B> : Parent<A, B>
    {
        public float MyProperty { get; set; }
        
        public float AnotherProperty
        {
            get { return MyProperty; }
        }
        
        
        public void MyMethod()
        {
            int[] values = {1, 2, 3, 4}; // No space within initializer brackets.
            int sum = 0;
            
            // Single line comment.
            for (int i = 0; i < values.Length; i++)
            {
                switch (i)
                {
                    case 3: return;
                    default:
                        sum += i > 2 ? 0 : 1;
                        break;
                }
            }
            
            i += (int)MyProperty; // No space after a type cast.
        }
    }

Naming conventions
------------------

Use **PascalCase** for all namespaces, type names and member level identifiers (i.e. methods, properties,
constants, events), except for private fields:

.. code-block:: csharp

    namespace ExampleProject
    {
        public class PlayerCharacter
        {
            public const float DefaultSpeed = 10f;
            
            public float CurrentSpeed { get; set; }
            
            protected int HitPoints;
            
            
            private void CalculateWeaponDamage()
            {
            }
        }
    }

Use **camelCase** for all other identifiers (i.e. local variables, method arguments), and use
an underscore (``_``) as a prefix for private fields (but not for methods or properties, as explained above):

.. code-block:: csharp

    private Vector3 _aimingAt; // Use a `_` prefix for private fields.
    
    
    private void Attack(float attackStrength)
    {
        Enemy targetFound = FindTarget(_aimingAt);
        targetFound?.Hit(attackStrength);
    }

There's an exception with acronyms which consist of two letters, like ``UI``, which should be written in
uppercase letters where PascalCase would be expected, and in lowercase letters otherwise.

Note that ``id`` is **not** an acronym, so it should be treated as a normal identifier:

.. code-block:: csharp

    public string Id { get; }
    
    public UIManager UI
    {
        get { return uiManager; }
    }

It is generally discouraged to use a type name as a prefix of an identifier, like ``string strText``
or ``float fPower``, for example. An exception is made, however, for interfaces, which
**should**, in fact, have an uppercase letter ``I`` prefixed to their names, like ``IInventoryHolder`` or ``IDamageable``.

Lastly, consider choosing descriptive names and do not try to shorten them too much if it affects
readability.

For instance, if you want to write code to find a nearby enemy and hit it with a weapon, prefer:

.. code-block:: csharp

    FindNearbyEnemy()?.Damage(weaponDamage);

Rather than:

.. code-block:: csharp

    FindNode()?.Change(wpnDmg);

Member variables
----------------

Don't declare member variables if they are only used locally in a method, as it
makes the code more difficult to follow. Instead, declare them as local
variables in the method's body.

Local variables
---------------

Declare local variables as close as possible to their first use. This makes it
easier to follow the code, without having to scroll too much to find where the
variable was declared.

Implicitly typed local variables
--------------------------------

Consider using implicitly typing (``var``) for declaration of a local variable, but do so
**only when the type is evident** from the right side of the assignment:

.. code-block:: csharp

    // You can use `var` for these cases:

    var direction = new Vector2(1, 0);

    var value = (int)speed;

    var text = "Some value";

    for (var i = 0; i < 10; i++)
    {
    }

    // But not for these:

    var value = GetValue();

    var velocity = direction * 1.5;

    // It's generally a better idea to use explicit typing for numeric values, especially with
    // the existence of the `real_t` alias in Godot, which can either be double or float
    // depending on the build configuration.

    var value = 1.5;

Other considerations
--------------------

 * Use explicit access modifiers.
 * Use properties instead of non-private fields.
 * Use modifiers in this order:
   ``public``/``protected``/``private``/``internal``/``virtual``/``override``/``abstract``/``new``/``static``/``readonly``.
 * Avoid using fully-qualified names or ``this.`` prefix for members when it's not necessary.
 * Remove unused ``using`` statements and unnecessary parentheses.
 * Consider using null-conditional operators or type initializers to make the code more compact.
 * Use safe cast when there is a possibility of the value being a different type, and use direct cast otherwise.
