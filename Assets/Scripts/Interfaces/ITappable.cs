using System.Collections.Generic;

public interface ITappable 
{
    void SetConnectedBlock(List<Block> connectedBlocks);
    void OnTap();

}
