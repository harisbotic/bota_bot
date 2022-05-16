var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DbContext>();
builder.Services.AddHttpClient<TelegramClient>();
var app = builder.Build();

app.MapPost("/botUpdate", (TgModels.Update command, DbContext db) =>
{
    //parse contract address from message
    //parse channel/sender id
    //save to database
    //note blockchain network
    throw new NotImplementedException();
});

app.MapPost("/alert", async (BlockchainWriteTransaction tx, TelegramClient tgClient, DbContext db) =>
{
    await tgClient.SendMessage("@devsBitv", GenerateAlertMessage(tx));
    //save event to database
    return true;
});

app.Run();

string GenerateAlertMessage(BlockchainWriteTransaction tx) => @$"‼️CONTRACT STATE CHANGED‼️

The function 💥 <b>{tx.FunctionName}</b> 💥 has just been executed and it changed the contract state on the blockchain.

Make sure to check it out 👀
<b>Contract:</b> {tx.Contract.ToLower()}

<b>TxHash:</b> <a href=""https://bscscan.com/tx/{tx.TxHash}"">Link</a>

<b>Timestamp:</b> {tx.TimestampUtc} UTC
 ";


public record BlockchainWriteTransaction(string Contract, string FunctionName, string TxHash, DateTime TimestampUtc);