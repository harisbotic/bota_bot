using System.Numerics;
using Nethereum.Web3;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace EthereumWatcher;

public class Watcher : BackgroundService
{
    public readonly string OneLoneVarContractAddress = "0x41Ea0601b4E7A9e65915E0252CD43a15131B6970";
    private readonly ILogger<Watcher> _logger;
    private readonly int _defaultStartingBlockNumber = 1;

    public Watcher(ILogger<Watcher> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var web3 = new Web3("http://127.0.0.1:7545");

        var storagePath = Path.ChangeExtension(Path.GetTempFileName(), "storage");
        var blockProgressRepository = new JsonBlockProgressRepository(
            async () => await Task.FromResult(File.Exists(storagePath)),
            async (json) => await File.WriteAllTextAsync(storagePath, json),
            async () => await File.ReadAllTextAsync(storagePath),
            _defaultStartingBlockNumber);

        var processor = web3.Processing.Blocks.CreateBlockProcessor(stepsConfiguration: steps =>
        {
            steps.TransactionStep.SetMatchCriteria(t =>
                t.Transaction.IsTo(OneLoneVarContractAddress) &&
                t.Transaction.IsTransactionForFunctionMessage<GiveNewValueFunction>());

            steps.TransactionStep.AddProcessorHandler(tx => WriteTransactionHandler.Handle(
                new BlockchainWriteTransaction(
                    Contract: OneLoneVarContractAddress,
                    FunctionName: "giveNewValue",
                    TxHash: tx.Transaction.TransactionHash,
                    TimestampUtc: DateTimeOffset.FromUnixTimeSeconds((long)tx.Block.Timestamp.Value).UtcDateTime)
                )
            );

        },
        blockProgressRepository: blockProgressRepository);

        var latestBlockProcessed = await blockProgressRepository.GetLastBlockNumberProcessedAsync();

        //start block processing
        await processor.ExecuteAsync(
            cancellationToken: cancellationToken,
            startAtBlockNumberIfNotProcessed: latestBlockProcessed);
    }
}

[Function("giveNewValue")]
public class GiveNewValueFunction : FunctionMessage
{
    [Parameter("uint256", "value")]
    public BigInteger Value { get; set; }
}