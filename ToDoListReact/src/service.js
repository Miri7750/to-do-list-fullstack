/////https://todolist-server-trnv.onrender.com
//import myAxios from './axiosConfiguration';
import axios from 'axios';

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
    const result = await myAxios.get(`/tasks`)
    if (result == undefined || result.data == undefined)
      return [];
    else
      return result.data;
  },

  addTask: async (name) => {
    const result = await myAxios.post(`/tasks`, { Id: 0, Name: name, IsCompelte: false })
    return {};
  },

  setCompleted: async (id, isComplete) => {
    const result = await myAxios.put(`/tasks/${id}`, { Id: id, Name: "", IsCompelte: isComplete });
    return {};
  },

  deleteTask: async (id) => {
    const result = myAxios.delete(`/tasks/${id}`);
    return {};
  }
};