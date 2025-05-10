INSERT INTO Wallet."Addresses" ("Id", "Street", "Number", "City", "Country")
VALUES
    (gen_random_uuid(), 'Rua A', 123, 'São Paulo', 'Brasil'),
    (gen_random_uuid(), 'Avenida B', 456, 'Rio de Janeiro', 'Brasil');


WITH addr AS (
    SELECT "Id" FROM Wallet."Addresses" LIMIT 2
)

INSERT INTO Wallet."Users" ("Id", "AddressId", "Name", "Email", "Password", "BirthDate")
VALUES
    (gen_random_uuid(), (SELECT "Id" FROM addr OFFSET 0 LIMIT 1), 'João Silva', 'joao@email.com',
     '$2a$11$F6g5YrNSvZ9KuNT0QRgKCO8zcOT0thLwvwwUwMvP6RHu1aJ.FXDsi', '1990-01-01'),
    (gen_random_uuid(), (SELECT "Id" FROM addr OFFSET 1 LIMIT 1), 'Maria Oliveira', 'maria@email.com',
     '$2a$11$4/9BCOCLfuoQsg7UmZecDeae8ARj3v5oE/I7jc94BlT8QUEHyZDqu', '1985-05-20');


WITH users AS (
    SELECT "Id", "Email" FROM Wallet."Users"
)


INSERT INTO Wallet."Wallets" ("Id", "UserId", "Balance")
VALUES
    (gen_random_uuid(), (SELECT "Id" FROM users WHERE "Email" = 'joao@email.com'), 1000.00),
    (gen_random_uuid(), (SELECT "Id" FROM users WHERE "Email" = 'maria@email.com'), 500.00);


WITH wallets AS (
    SELECT w."Id", u."Email"
    FROM Wallet."Wallets" w
             JOIN Wallet."Users" u ON w."UserId" = u."Id"
)

INSERT INTO Wallet."WalletTransactions" (
    "FromWalletId", "FromEmail", "ToWalletId", "ToEmail", "Amount", "Transaction", "Status"
)
VALUES
(
    (SELECT "Id" FROM wallets WHERE "Email" = 'joao@email.com'),
    'joao@email.com',
    (SELECT "Id" FROM wallets WHERE "Email" = 'joao@email.com'),
    'joao@email.com',
    250.00,
    'Deposit',
    1
),
(
    (SELECT "Id" FROM wallets WHERE "Email" = 'joao@email.com'),
    'joao@email.com',
    (SELECT "Id" FROM wallets WHERE "Email" = 'maria@email.com'),
    'maria@email.com',
    150.00,
    'Transfer',
    1
),
(
    (SELECT "Id" FROM wallets WHERE "Email" = 'maria@email.com'),
    'maria@email.com',
    (SELECT "Id" FROM wallets WHERE "Email" = 'maria@email.com'),
    'maria@email.com',
    300.00,
    'Deposit',
    1
),
(
    (SELECT "Id" FROM wallets WHERE "Email" = 'maria@email.com'),
    'maria@email.com',
    (SELECT "Id" FROM wallets WHERE "Email" = 'joao@email.com'),
    'joao@email.com',
    50.00,
    'Transfer',
    1
);