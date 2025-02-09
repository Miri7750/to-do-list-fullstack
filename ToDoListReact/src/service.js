
import myAxios from './axiosConfiguration';

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