:- use_module(library(http/json)).

% Declare dynamic predicates for transactions and budget goals
:- dynamic transaction/4.
:- dynamic budget_goal/3.
:- dynamic transaction_marker/0.

% Set up the marker initially
transaction_marker.

% Facts for transactions, categories, and budget goals
transaction(john, groceries, 50, '2023-12-01').
transaction(john, dining_out, 30, '2023-12-02').
transaction(alice, groceries, 40, '2023-12-01').
transaction(alice, shopping, 100, '2023-12-03').

budget_goal(john, groceries, 150).
budget_goal(alice, shopping, 200).

% Predicate to add a transaction
% add_transaction(User, Category, Amount, Date) :-
%     assertz(transaction(User, Category, Amount, Date)),
%     format('Transaction added for ~w: ~w ~w on ~w.~n', [User, Amount, Category, Date]),
%     tell('/home/diti/Desktop/finances.pl'),
%     listing(transaction),
%     told.

% Predicate to add a transaction
add_transaction(User, Category, Amount, Date) :-
    assertz(transaction(User, Category, Amount, Date)),
    (   transaction_marker ->
        assertz(transaction_marker(false)),
        open('/home/diti/Desktop/finances.pl', append, Stream),
        write(Stream, 'transaction('),
        write(Stream, User),
        write(Stream, ', '),
        write(Stream, Category),
        write(Stream, ', '),
        write(Stream, Amount),
        write(Stream, ', '''),
        write(Stream, Date),
        write(Stream, ''').'),
        nl(Stream),
        close(Stream)
    ;   true
    ).

% Predicate to set a budget goal
set_budget_goal(User, Category, Budget) :-
    retractall(budget_goal(User, Category, _)),
    assertz(budget_goal(User, Category, Budget)),
    (   transaction_marker ->
        assertz(transaction_marker(false)),
        open('/home/diti/Desktop/finances.pl', append, Stream),
        write(Stream, 'budget_goal('),
        write(Stream, User),
        write(Stream, ', '),
        write(Stream, Category),
        write(Stream, ', '),
        write(Stream, Budget),
        write(Stream, ').'),
        nl(Stream),
        close(Stream)
    ;   true
    ),
    format('Budget goal set for ~w: ~w ~w.~n', [User, Budget, Category]).

% Predicate to inquire about financial status
inquire_financial_status(User) :-
    format('Financial Status for ~w:~n', [User]),
    show_transactions(User),
    show_budget_goals(User).

% Helper predicate to display transactions as JSON
show_transactions_json(User) :-
    findall(json([category=Category, amount=Amount, date=Date]),
            transaction(User, Category, Amount, Date),
            Transactions),
    json_write(current_output, Transactions).

% Main predicate to query transactions and print the result
show_transactions(User) :-
    show_transactions_json(User),
    format('.\n').

% Predicate to remove a specific transaction for a user
remove_transaction(User, Category, Amount, Date) :-
    clause(transaction(User, Category, Amount, Date), _),
    retract(transaction(User, Category, Amount, Date)),
    save_user_data_except(User).

% Predicate to save user data except for a specific user
save_user_data_except(User) :-
    open('/home/diti/Desktop/finances.pl', read, ReadStream),
    open('/home/diti/Desktop/finances_tmp.pl', write, WriteStream),
    save_transactions_except(ReadStream, WriteStream, User),
    close(ReadStream),
    close(WriteStream),
    rename_file('/home/diti/Desktop/finances_tmp.pl', '/home/diti/Desktop/finances.pl').

% Helper predicate to save transactions except for a specific user
save_transactions_except(ReadStream, WriteStream, User) :-
    repeat,
    read(ReadStream, Term),
    (   Term == end_of_file,
        !
    ;   (   Term = transaction(OtherUser, _, _, _),
            User \= OtherUser,
            writeq(WriteStream, Term),
            write(WriteStream, '.'),
            nl(WriteStream),
            fail
        ;   true
        ),
        fail
    ).

% Predicate to remove a specific budget goal for a user
remove_budget_goal(User, Category, Budget) :-
    retract(budget_goal(User, Category, Budget)),
    save_user_data_except(User).

% Modified helper predicate to save budget goals except for a specific user
save_budget_goals_except(ReadStream, WriteStream, User) :-
    repeat,
    read(ReadStream, Term),
    (   Term == end_of_file,
        !
    ;   (   Term = budget_goal(OtherUser, _, _),
            User \= OtherUser,
            writeq(WriteStream, Term),
            write(WriteStream, '.'),
            nl(WriteStream),
            fail
        ;   true
        ),
        fail
    ).

% Helper predicate to convert a single transaction to JSON
transaction_to_json(transaction(Category, Amount, Date), json{category:Category, amount:Amount, date:Date}).

% Helper predicate to display a single transaction
display_transaction(transaction(Category, Amount, Date)) :-
    format('- ~w ~w on ~w~n', [Amount, Category, Date]).

show_transactions_budget(User) :-
    findall(json([user = User, category=Category, budget=Budget]),
            budget_goal(User, Category, Budget),
            Goals),
    json_write(current_output, Goals).

% Helper predicate to display budget goals
show_budget_goals(User) :-
    format('Budget Goals:~n'),
    budget_goal(User, Category, Budget),
    format('- ~w ~w ~w~n', [Budget, Category, User]),
    fail.
show_budget_goals(_).
transaction(baba, groceries, 420, '2023-12-10').
transaction(baba, shopping, 220, '2023-12-10').
transaction(mami, shopping, 155, '2023-12-01').
budget_goal(baba, groceries, 500).
transaction(diti, shopping, 155, '2023-12-01').
budget_goal(baba, shopping, 150).
transaction(diti, shopping, 350, '2022-12-02').
transaction(diti, shopping, 350, '2022-12-02').
budget_goal(diti, groceries, 2500).
budget_goal(diti, shopping, 1500).
budget_goal(diti, tech, 12000).
budget_goal(diti, home, 1200).
