
import axios from 'axios';

// axios.defaults.baseURL="http://localhost:5220"
axios.defaults.baseURL = process.env.REACT_APP_API_URL;
console.log(process.env.REACT_APP_API_URL)

axios.interceptors.response.use(
  response => response,
  error => {
    console.error('Axios Error:', error.response ? error.response.data : error.message);
    //return Promise.reject(error); // דחוף את השגיאה כדי שתוכל לטפל בה במקום אחר
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get(`/items`);
    console.log(result, "get all");
    if (result == undefined || result.data == undefined)
      return [];
    else
      return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name);
    const result = await axios.post(`/items`, { id: 0, name: name, isComplete: false });
    console.log(result, "add");

    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete: isComplete });
    const {data} = await axios.get(`/items/${id}`);
    console.log(data, "get task to update");
    const newTask= { id: id, name: data.name, isCompelete: isComplete }
    console.log(newTask, "new task to update");
    const result = await axios.put(`/items/${id}`,newTask);
    console.log(result, "update");
    return result.data;
  },
  deleteTask: async (id) => {
    console.log('deleteTask');
    const result = await axios.delete(`/items/${id}`);
    console.log(result, "delete");

    return result.data;
  }

};
