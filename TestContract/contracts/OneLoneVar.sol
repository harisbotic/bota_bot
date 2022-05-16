// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

contract OneLoneVar {
   uint loneVariable;  
  
   function giveNewValue(uint value) public {
     loneVariable = value;   
   }
   function get() view public returns (uint) {
     return loneVariable;   
   } 
}