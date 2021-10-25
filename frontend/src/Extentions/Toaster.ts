import Swal from "sweetalert2";


export async function ErrorMessage(message:string){
  return Swal.fire({
    icon: 'error',     
     text:  'Oops...\nSomething went wrong!\n'
   });
 }