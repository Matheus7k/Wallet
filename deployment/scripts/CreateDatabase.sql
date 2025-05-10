CREATE SCHEMA IF NOT EXISTS Wallet;

CREATE TABLE IF NOT EXISTS Wallet."Addresses" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "Street" VARCHAR(255) NOT NULL,
    "Number" INTEGER NOT NULL,
    "City" VARCHAR(100) NOT NULL,
    "Country" VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS Wallet."Users" (
    "Id" UUID PRIMARY KEY,
    "AddressId" UUID NOT NULL,
    "Name" VARCHAR(255) NOT NULL,
    "Email" VARCHAR(255) NOT NULL,
    "Password" VARCHAR(500) NOT NULL,
    "BirthDate" DATE NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_user_address FOREIGN KEY ("AddressId")
     REFERENCES Wallet."Addresses"("Id") ON DELETE RESTRICT,

    CONSTRAINT uk_user_email UNIQUE ("Email")
);

CREATE TABLE IF NOT EXISTS Wallet."Wallets" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "UserId" UUID NOT NULL,
    "Balance" DECIMAL(18, 2) NOT NULL DEFAULT 0.00,
    "IsActive" BOOLEAN NOT NULL DEFAULT TRUE,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_wallet_user FOREIGN KEY ("UserId")
      REFERENCES Wallet."Users"("Id") ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS Wallet."WalletTransactions" (
    "Id" UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    "FromWalletId" UUID NOT NULL,
    "FromEmail" VARCHAR(255) NOT NULL,
    "ToWalletId" UUID NOT NULL,
    "ToEmail" VARCHAR(255) NOT NULL,
    "Amount" DECIMAL(18, 2) NOT NULL,
    "Transaction" VARCHAR(20) NOT NULL,
    "Status" INT NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    "CreatedAtUtc" TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_transaction_wallet FOREIGN KEY ("FromWalletId")
        REFERENCES Wallet."Wallets"("Id")
);


CREATE INDEX IF NOT EXISTS idx_user_name ON Wallet."Users" ("Email");
CREATE INDEX IF NOT EXISTS idx_wallets_id ON Wallet."Wallets" ("Id");
CREATE INDEX IF NOT EXISTS idx_wallets_user_id ON Wallet."Wallets" ("UserId");
CREATE INDEX IF NOT EXISTS idx_wallet_transactions_from_wallet_id ON Wallet."WalletTransactions" ("FromWalletId");
CREATE INDEX IF NOT EXISTS idx_wallet_transactions_to_wallet_id ON Wallet."WalletTransactions" ("ToWalletId");
CREATE INDEX IF NOT EXISTS idx_wallet_transactions_created_at ON Wallet."WalletTransactions" ("CreatedAt" ASC);