import { PipeTransform, Pipe } from "@angular/core";

@Pipe({
    name: 'members'
})
export class EnumMembersPipe implements PipeTransform {

    transform(value: any, ...args: any[]) : any {
      let members = []; 
      for(let member in value) { 
        let isValueProperty = parseInt(member, 10) >= 0; 
        if(isValueProperty) { 
            members.push({key: member, value: value[member]})
        }
      }
      return members; 
    }
}